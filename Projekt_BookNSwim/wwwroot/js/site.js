// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write yiour JavaScript code.


// this code disables the button for the searchbar if both fields are empty
let searchInp = document.querySelectorAll(".searchinput");

searchInp.forEach(inp => {
    inp.addEventListener("keyup", (e) => {
        document.getElementById("searchBtn").disabled = (e.target.value.length == 0) ? true : false;
    });
});