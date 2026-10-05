const admin = require("firebase-admin");
const fs = require("fs");

require("dotenv").config();

let serviceAccount;

const credentialPath = process.env.FIREBASE_CREDENTIAL_PATH;

if (credentialPath && fs.existsSync(credentialPath)) {
    serviceAccount = JSON.parse(
        fs.readFileSync(credentialPath, "utf8")
    );
}

if (!serviceAccount) {
    const credentialJson = process.env.FIREBASE_CREDENTIAL_JSON;

    if (credentialJson) {
        serviceAccount = JSON.parse(credentialJson);
    }
}

if (!serviceAccount) {
    throw new Error(
        "Firebase credentials were not found. " +
        "Set FIREBASE_CREDENTIAL_PATH or FIREBASE_CREDENTIAL_JSON."
    );
}

admin.initializeApp({
    credential: admin.credential.cert(serviceAccount)
});

const db = admin.firestore();

module.exports = {
    admin,
    db
};