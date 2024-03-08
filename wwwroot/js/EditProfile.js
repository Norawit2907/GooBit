document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('.button-container form');

    form.addEventListener('submit', async function (event) {
        event.preventDefault();

        const firstName = document.querySelector('.first-name').value;
        const lastName = document.querySelector('.last-name').value;
        const email = document.querySelector('.email').value;
        const description = document.querySelector('.description').value;
        const password = document.querySelector('.password').value;
        const confirmPassword = document.querySelector('.confirm-password').value;

        const data = {
            firstName: firstName,
            lastName: lastName,
            email: email,
            description: description,
            password: password,
            confirmPassword: confirmPassword
        };

    });
});

// EditProfile.js

// Function to handle file input change and update the image
var displaySelectedImage = function (input) {
    var image = document.getElementById("output");
    var file = input.files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            image.src = e.target.result;
        };

        reader.readAsDataURL(file);
    }
};

// Function to handle file input change and update the image in case of profile picture change
var loadFile = function (event) {
    var image = document.getElementById("output");
    image.src = URL.createObjectURL(event.target.files[0]);
};
