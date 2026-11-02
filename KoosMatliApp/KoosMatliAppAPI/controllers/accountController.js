'use strict';


function createAccountController({ auth, config, fetchImpl }) {
  const resetPassword = async (req, res) => {
    if (!config.webApiKey) return res.status(503).json({ message: 'Configure FIREBASE_WEB_API_KEY in the backend before sending password reset emails.' });
    const user = await auth.getUser(req.user.uid); //(Firebase Documentation, 2026).
    if (!user.email || !user.providerData.some(p => p.providerId === 'password')) return res.status(400).json({ message: 'Password reset is available for email/password accounts.' });
    let response;
    try {
      response = await fetchImpl(`https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=${encodeURIComponent(config.webApiKey)}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ requestType: 'PASSWORD_RESET', email: user.email }), signal: AbortSignal.timeout(30000) }); //(Google Cloud, 2026a).
    } catch { return res.status(502).json({ message: 'Firebase could not send the reset email. Try again later.' }); } //(MDN Web Docs, 2026).
    if (!response.ok) return res.status(502).json({ message: 'Firebase could not send the reset email. Check the Web API key and email/password provider, or try again later.' });
    res.sendStatus(204);
  };
  return { resetPassword };
}
module.exports = createAccountController;

/*Reference List

Google Cloud, "Firebase Auth REST API Reference - sendOobCode," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/reference/rest/auth#section-send-password-reset-email

Firebase Documentation, "Manage Users in Firebase Auth Admin SDK," Google Cloud, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/manage-users

MDN Web Docs, "Fetch API and AbortSignal.timeout," Mozilla Developer Network, 2026. [Online]. Available: https://developer.mozilla.org/en-US/docs/Web/API/AbortSignal/timeout
*/