'use strict';


function createAccountController({ auth, config, fetchImpl }) {
  const resetPassword = async (req, res) => {
    if (!config.webApiKey) return res.status(503).json({ message: 'Configure FIREBASE_WEB_API_KEY in the backend before sending password reset emails.' });
    const user = await auth.getUser(req.user.uid);
    if (!user.email || !user.providerData.some(p => p.providerId === 'password')) return res.status(400).json({ message: 'Password reset is available for email/password accounts.' });
    let response;
    try {
      response = await fetchImpl(`https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=${encodeURIComponent(config.webApiKey)}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ requestType: 'PASSWORD_RESET', email: user.email }), signal: AbortSignal.timeout(30000) });
    } catch { return res.status(502).json({ message: 'Firebase could not send the reset email. Try again later.' }); }
    if (!response.ok) return res.status(502).json({ message: 'Firebase could not send the reset email. Check the Web API key and email/password provider, or try again later.' });
    res.sendStatus(204);
  };
  return { resetPassword };
}
module.exports = createAccountController;
