# Koos Matli Admin API

This backend stores school records/settings in Cloud Firestore and enforces admin access using verified Firebase ID tokens supplied by the team's authentication module. It does not implement login, account creation, password reset or token refresh.

See the root START-HERE.md for Firebase configuration, admin custom-claim expectations, the MAUI handoff and platform-file merge instructions. Existing API routes, PascalCase stored Firestore fields and numeric record IDs are retained. The settings UID is taken from the verified token. Demo seeding is disabled by default and supported only in Development; it does not provision Authentication accounts or real files. Firestore server access uses IAM; the optional firestore.rules file denies direct client access for an API-only architecture and should not be deployed over unrelated team features without checking their access needs.

The Firestore store allocates IDs transactionally, skips existing IDs, reserves profile emails and uses UTC timestamp converters. Explicit controller validation errors are localized in English/Setswana/Sepedi. Settings include language, theme, notification preference, displayName, phoneNumber and confirmSensitiveActions. Staff read endpoints include classes and subjects. List endpoints currently load full collections; pagination is needed for large datasets. School documents store metadata, not uploaded files. Announcements are not implemented.

Current verification results and untested live/native behavior are recorded in the root VERIFICATION.md. The portable backend smoke test is in tools/smoke_test.py.
