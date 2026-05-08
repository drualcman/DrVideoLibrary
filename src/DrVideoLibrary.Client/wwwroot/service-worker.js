const cacheNamePrefix = 'offline-cache-';
const cacheVersion = '1';
const cacheName = `${cacheNamePrefix}${cacheVersion}`;
const noImageUrl = 'https://drualcman.blob.core.windows.net/content/shotup-noimage.png';

const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.woff2$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/];

const offlineAssetsExclude = [
    /^\/api\//,
    /^\/push\//
];

const cacheAllowedExternalOrigins = [
    'https://cdn.jsdelivr.net',
    'https://cdnjs.cloudflare.com',
    'https://drualcman.blob.core.windows.net'
];

self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => {
    if (event.request.method === 'GET') {
        event.respondWith(onFetch(event));
    }
});

async function onInstall(event) {
    console.log('Installing service worker, cache:', cacheName);
    self.skipWaiting();
    const cache = await caches.open(cacheName);
    await Promise.allSettled([
        cache.add('/').catch(err => console.warn('Failed to pre-cache index:', err)),
        cache.add(noImageUrl).catch(err => console.warn('Failed to pre-cache noimage:', err))
    ]);
}

async function onActivate(event) {
    console.log('Activating service worker, cache:', cacheName);
    const cacheKeys = await caches.keys();
    await Promise.all(
        cacheKeys
            .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
            .map(key => caches.delete(key))
    );
    await clients.claim();
}

async function onFetch(event) {
    const request = event.request;
    const url = new URL(request.url);
    const isSameOrigin = url.origin === self.location.origin;
    const isAllowedExternal = cacheAllowedExternalOrigins.includes(url.origin);
    const isNavigation = request.mode === 'navigate';

    // Navigation requests: always return index.html (Blazor SPA routing)
    if (isNavigation) {
        const cache = await caches.open(cacheName);
        try {
            const networkResponse = await fetch('/');
            if (networkResponse.ok) {
                // Always update the cache with the latest index.html
                await cache.put('/', networkResponse.clone());
                return networkResponse;
            }
        } catch {
            // Network failed, try cache
            const cachedIndex = await cache.match('/');
            if (cachedIndex) {
                return cachedIndex;
            }
            return new Response('<html><body><h1>Offline</h1><p>Please reconnect to continue.</p></body></html>', {
                status: 200,
                headers: { 'Content-Type': 'text/html' }
            });
        }
    }

    // Skip non-cacheable origins
    if (!isSameOrigin && !isAllowedExternal) {
        try {
            return await fetch(request);
        } catch {
            return new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    // Skip excluded paths
    const isExcluded = isSameOrigin && offlineAssetsExclude.some(pattern => pattern.test(url.pathname));
    if (isExcluded) {
        try {
            return await fetch(request);
        } catch {
            return new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    // User images fallback
    const isUserImage = url.pathname.startsWith('/movies/');
    if (isUserImage) {
        try {
            return await fetch(request);
        } catch {
            const cache = await caches.open(cacheName);
            const fallback = await cache.match(noImageUrl);
            return fallback || new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    // Cache first for all other assets
    const shouldCache = offlineAssetsInclude.some(pattern => pattern.test(url.pathname));
    if (!shouldCache) {
        try {
            return await fetch(request);
        } catch {
            return new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);
    if (cachedResponse) {
        return cachedResponse;
    }

    try {
        const networkResponse = await fetch(request);
        if (networkResponse.ok) {
            await cache.put(request, networkResponse.clone());
        }
        return networkResponse;
    } catch {
        return new Response('', { status: 503, statusText: 'Offline' });
    }
}