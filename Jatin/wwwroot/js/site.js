// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showPic(e) {
    var reader = new FileReader();
    reader.onload = function () {
        document.getElementById("profilePicPreview").style.display = "flex";
        document.getElementsByClassName("formBody")[0].style.height = "88vh";
        document.getElementById("profilePicPreview").innerHTML = `<img src="${reader.result}" height="100">`;
    }
    reader.readAsDataURL(e.target.files[0]);
}