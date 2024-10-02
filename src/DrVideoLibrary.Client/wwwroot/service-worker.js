const cacheName = 'app-cache-v1';
// Define static resources manually
const resourcesToCache = [
    '/',
    '/index.html',
    '/service-worker.js',
    '/manifest.json',
    '/favicon.ico',
    '/appsettings.json',
    '/css/mago.css',
    '/img/Logo512.png',
    '/_content/DrVideoLibrary.Razor/css/app.css',
    '/DrVideoLibrary.Client.styles.css',
    '/_content/DrVideoLibrary.Razor/DrVideoLibrary.Razor.bundle.scp.css',
    '/_framework/blazor.webassembly.js',
    '/_framework/aspnetcore-browser-refresh.js',
    '/_framework/dotnet.js',
    '/_framework/blazor.boot.json',
    '/_framework/dotnet.runtime.8.0.8.80cvijctdx.js',
    '/_framework/dotnet.native.8.0.8.b0ph3wcvcn.js',
    '/_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js',
    '/offline.html'
];

// Install event to cache static resources
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(cacheName).then(cache => {
            const addPromises = resourcesToCache.map(resource => {
                return fetch(resource).then(response => {
                    if (!response.ok) {
                        throw new Error(`Failed to fetch ${resource}: ${response.statusText}`);
                    }
                    return cache.put(resource, response);
                }).catch(error => {
                    console.error('Error caching resource:', error);
                });
            });
            return Promise.allSettled(addPromises);
        })
            .then(() => {
                // Fetch `blazor.boot.json` to cache dynamic resources
                return fetch('/_framework/blazor.boot.json')
                    .then(response => response.json())
                    .then(blazorConfig => {
                        // Add wasmNative, jsModuleNative, jsModuleRuntime, and assembly resources
                        for (const wasmKey in blazorConfig.resources.wasmNative) {
                            resourcesToCache.push(`/_framework/${wasmKey}`);
                        }
                        for (const jsNativeKey in blazorConfig.resources.jsModuleNative) {
                            resourcesToCache.push(`/_framework/${jsNativeKey}`);
                        }
                        for (const jsRuntimeKey in blazorConfig.resources.jsModuleRuntime) {
                            resourcesToCache.push(`/_framework/${jsRuntimeKey}`);
                        }
                        for (const assemblyKey in blazorConfig.resources.assembly) {
                            resourcesToCache.push(`/_framework/${assemblyKey}`);
                        }
                        // Handle satelliteResources if they exist
                        if (blazorConfig.resources.satelliteResources) {
                            for (const culture in blazorConfig.resources.satelliteResources) {
                                for (const satelliteKey in blazorConfig.resources.satelliteResources[culture]) {
                                    resourcesToCache.push(`/_framework/${satelliteKey}`);
                                }
                            }
                        }
                        const addDynamicPromises = resourcesToCache.map(resource => {
                            return fetch(resource).then(response => {
                                if (!response.ok) {
                                    throw new Error(`Failed to fetch ${resource}: ${response.statusText}`);
                                }
                                return caches.open(cacheName).then(cache => {
                                    return cache.put(resource, response);
                                });
                            }).catch(error => {
                                console.error('Error caching dynamic resource:', error);
                            });
                        });
                        return Promise.allSettled(addDynamicPromises);
                    });
            })
    );
});

// Activate event to update cache
self.addEventListener('activate', event => {
    const cacheWhitelist = [cacheName];
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cache => {
                    if (!cacheWhitelist.includes(cache)) {
                        return caches.delete(cache);
                    }
                })
            );
        })
    );
});

// Fetch event to serve resources from cache
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request, { ignoreSearch: true }).then(cachedResponse => {
            return cachedResponse || fetch(event.request).then(networkResponse => {
                // Store valid responses in cache
                if (networkResponse && networkResponse.status === 200 && networkResponse.type === 'basic') {
                    const responseToCache = networkResponse.clone();
                    caches.open(cacheName).then(cache => {
                        cache.put(event.request, responseToCache);
                    });
                }
                return networkResponse;
            }).catch(error => {
                console.error('Error fetching resource:', error);
                // Fallback to offline page if resource is not cached
                return caches.match('/offline.html');
            });
        })
    );
});
