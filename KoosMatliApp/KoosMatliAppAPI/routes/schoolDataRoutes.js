const express = require("express");

const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize"); //(ExpressJS, 2026a).

const {
    linkChildToParent
} = require("../controllers/schoolDataController");

const router = express.Router();

router.use(authenticate, authorize("admin")); //(Open Web Application Security Project [OWASP], 2026).

router.post(
    "/parents/:parentUid/children", //(Fielding, 2000).
    linkChildToParent
);

module.exports = router;

/*Reference List

ExpressJS Documentation, "Express Router Middleware and Route Stacking," ExpressJS, 2026. [Online]. Available: https://expressjs.com/en/guide/routing.html

Open Web Application Security Project (OWASP), "Access Control Architecture and RBAC," OWASP Foundation, 2026. [Online]. Available: https://owasp.org/www-community/Access_Control

R. T. Fielding, Architectural Styles and the Design of Network-based Software Architectures, Doctoral dissertation, University of California, Irvine, 2000.
*/
