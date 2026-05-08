const cacheNamePrefix = 'offline-cache-';
const cacheVersion = '1';
const cacheName = `${cacheNamePrefix}${cacheVersion}`;

const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.woff2$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/];

// Paths that should never be cached (same-origin only)
const offlineAssetsExclude = [
    /^\/movies\//,      // user uploaded images
    /^\/api\//,         // api calls
    /^\/push\//         // push notifications
];

// External domains allowed to be cached
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
}

async function onActivate(event) {
    console.log('Activating service worker, cache:', cacheName);
    const cacheKeys = await caches.keys();
    await Promise.all(
        cacheKeys
            .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
            .map(key => {
                console.log('Deleting old cache:', key);
                return caches.delete(key);
            })
    );
    await clients.claim();
}

async function onFetch(event) {
    const request = event.request;
    const url = new URL(request.url);
    const isSameOrigin = url.origin === self.location.origin;
    const isAllowedExternal = cacheAllowedExternalOrigins.includes(url.origin);

    // Skip everything that is not same-origin or explicitly allowed external
    if (!isSameOrigin && !isAllowedExternal) {
        try {
            return await fetch(request);
        } catch {
            return new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    // Skip excluded paths (same-origin only)
    const isExcluded = isSameOrigin && offlineAssetsExclude.some(pattern => pattern.test(url.pathname));
    if (isExcluded) {
        try {
            return await fetch(request);
        } catch {
            return new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    // Cache navigation requests (index.html) explicitly
    const isNavigation = request.mode === 'navigate';

    // Only cache files matching the include patterns or navigation
    const shouldCache = isNavigation || offlineAssetsInclude.some(pattern => pattern.test(url.pathname));
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
        // Offline and not in cache
        if (isNavigation) {
            // Try to return cached index.html as fallback
            const indexResponse = await cache.match('/');
            if (indexResponse) {
                return indexResponse;
            }
        }
        return new Response('Offline - resource not cached', { status: 503, statusText: 'Offline' });
    }
}