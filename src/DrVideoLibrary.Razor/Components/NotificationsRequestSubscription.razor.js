async function registerServiceWorker(swPath, swScope) {
    try {
        const registration = await navigator.serviceWorker.register(swPath, { scope: swScope });
        console.info('Service worker registered with scope:', swScope);
        return registration;
    } catch (e) {
        console.error('Error registering service worker:', e);
        return null;
    }
}
async function requestNotificationPermission() {
    try {
        const permission = await Notification.requestPermission();
        if (permission !== "granted") {
            console.warn('Notification permission not granted');
            return null;
        }
        return permission;
    } catch (e) {
        console.error('Error requesting notification permission:', e);
        return null;
    }
}

async function setupAndSubscribe(applicationServerPublicKey, swScope = '/', file) {
    if (!('serviceWorker' in navigator)) {
        console.warn('Service workers are not supported by this browser.');
        return null;
    }

    if (!('Notification' in window)) {
        console.warn('Notifications are not supported by this browser.');
        return null;
    }

    const swPath = `${swScope}${file}`;
    const registration = await registerServiceWorker(swPath, swScope);
    if (!registration) return null;

    const permission = await requestNotificationPermission();
    if (!permission) return null;

    try {
        const subscription = await requestSubscription(registration, applicationServerPublicKey);
        return subscription;
    } catch (e) {
        console.error('Error requesting subscription:', e);
        return null;
    }
}

try {
    navigator.serviceWorker.addEventListener('message', function (event) {
        try {
            if (event.data.type === 'CATALOG') {
                console.log('CATALOG', event.data.update);
                localStorage.setItem(event.data.type, event.data.update);
            }
        } catch (e) {
            console.warn('Error processing service worker message:', e);
        }
    });
} catch (e) {
    console.warn('Error adding service worker event listener:', e);
}

function getFingerPrint() {
    return new Promise((resolve, reject) => {
        try {
            let userId = localStorage.getItem("userId");
            if (userId === undefined || userId == 'undefined' || userId === null) {
                const data = `${navigator.userAgent}|${new Intl.DateTimeFormat().resolvedOptions().timeZone}|${navigator.language}`;
                userId = data;
                localStorage.setItem("userId", userId);
            }
            resolve(userId);
        } catch (e) {
            console.warn('Error getting fingerprint:', e);
            reject(e);
        }
    });
}

function hasNotGrandNotifications() {
    if (!('Notification' in window)) {
        console.warn('Notifications are not supported by this browser.');
        return false;
    }
    else {
        try {
            return Notification.permission !== "granted";
        } catch (e) {
            console.warn('Error checking notification permission:', e);
            return true;
        }
    }
}

export { setupAndSubscribe, getFingerPrint, hasNotGrandNotifications }

async function requestSubscription(registration, applicationServerPublicKey) {
    try {
        const existingSubscription = await registration.pushManager.getSubscription();
        if (!existingSubscription) {
            const newSubscription = await subscribe(registration, applicationServerPublicKey);
            if (newSubscription) {
                return {
                    url: newSubscription.endpoint,
                    p256dh: arrayBufferToBase64(newSubscription.getKey('p256dh')),
                    auth: arrayBufferToBase64(newSubscription.getKey('auth'))
                };
            }
        }
        return null;
    } catch (e) {
        console.error('Error requesting subscription:', e);
        return null;
    }
}

async function subscribe(registration, applicationServerPublicKey) {
    try {
        return await registration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: applicationServerPublicKey
        });
    } catch (error) {
        console.error('Error subscribing to push:', error);
        throw error;
    }
}

function arrayBufferToBase64(buffer) {
    try {
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    } catch (e) {
        console.error('Error converting array buffer to base64:', e);
        return null;
    }
}
