﻿self.addEventListener("install", async event => {
    console.log("Installing service worker...");
    self.skipWaiting();
});

self.addEventListener("fetch", event => {
    //code for ofline management
    return null;
});

self.addEventListener('push', event => {
    const payLoad = event.data.json();
    event.waitUntil(self.registration.showNotification('DrMovies Video', {
        body: payLoad.message,
        icon: 'img/Logo512.png',
        vibrate: [100, 50, 100],
        data: { url: payLoad.url }
    }));
});

self.addEventListener("notificationclick", event => {
    event.notification.close();
    event.waitUntil(clients.openWindow(event.notification.data.url));
});