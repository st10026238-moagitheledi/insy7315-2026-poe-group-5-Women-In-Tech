const { initializeApp, cert, getApps } = require("firebase-admin/app");
const { getFirestore } = require("firebase-admin/firestore");
const { getAuth } = require("firebase-admin/auth");
const { getStorage } = require("firebase-admin/storage");

const serviceAccount = require("../koosmatliprimaryschoolsystem-firebase-adminsdk-fbsvc-60df4d9328.json");

const firebaseApp =
    getApps().length > 0
        ? getApps()[0]
        : initializeApp({
              credential: cert(serviceAccount)
          });

const db = getFirestore(firebaseApp);
const auth = getAuth(firebaseApp);
const storage = getStorage(firebaseApp);

module.exports = {
    firebaseApp,
    db,
    auth,
    storage
};
