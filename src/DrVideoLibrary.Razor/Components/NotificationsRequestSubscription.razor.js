async function setupAndSubscribe(applicationServerPublicKey, swScope = '/', file) {
    if (!('serviceWorker' in navigator)) {
        console.warn('Service workers are not supported by this browser.');
        return null;
    }

    try {
        const swPath = `${swScope}${file}`; 
        const adjustedPath = swPath.startsWith('/') ? swPath.substring(1) : swPath; 
        const registration = await navigator.serviceWorker.register(adjustedPath, { scope: swScope });
        console.info('Notification Service worker registered successfully with scope:', swScope);

        await navigator.serviceWorker.ready;
        console.info('Notification Service Worker is ready');
        const permission = await Notification.requestPermission();
        if (permission !== "granted") {
            console.warn('Notification permission not granted');
            return null;
        }

        const subscription = await requestSubscription(registration, applicationServerPublicKey);
        return subscription;
    } catch (e) {
        console.error('Error during SW registration or notification subscription:', e);
        return null;
    }
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
            reject(e);
        }
    });
}

function hasNotGrandNotifications() {
    return Notification.permission !== "granted";
}

navigator.serviceWorker.addEventListener('message', function (event) {
    if (event.data.action === 'updateLocalStorage') {
        localStorage.setItem(event.data.key, event.data.value);
    }
});

export { setupAndSubscribe, getFingerPrint, hasNotGrandNotifications }

async function requestSubscription(registration, applicationServerPublicKey) {
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
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}
