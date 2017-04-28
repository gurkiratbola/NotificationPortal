window.onload = function () {
    setTimezoneCookie();
};

function setTimezoneCookie() {
    document.cookie = "timezoneoffset=" + (-new Date().getTimezoneOffset() / 60);
}