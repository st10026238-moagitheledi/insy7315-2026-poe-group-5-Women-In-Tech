const getHome = (req, res) => {
    res.json({
        message: "Welcome to the Koos Matli Primary School Website API",
        school: "Koos Matli Primary School",
        motto: "Thuto ke Lesedi"
    });
};

module.exports = {
    getHome
};