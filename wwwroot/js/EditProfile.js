function handleImagePreview(input) {
    const previewContainer = document.getElementById('image-preview');
    previewContainer.innerHTML = '';
    const files = input.files;

    if (files && files.length > 0) {
        const file = files[0];
        const reader = new FileReader();

        reader.onload = function (e) {
            const image = document.createElement('img');
            image.src = e.target.result;
            image.classList.add("profile-image");
            previewContainer.appendChild(image);
        };
        reader.readAsDataURL(file);
        
    } else if (oldimg) {
        const oldImagePreview = document.createElement('img');
        oldImagePreview.src = basePath+oldimg;
        oldImagePreview.classList.add("profile-image");
        previewContainer.appendChild(oldImagePreview);
        console.log(oldImagePreview);
    }
}

window.onload = function () {
    handleImagePreview(document.getElementById('proImage'));
};

document.getElementById('proImage').addEventListener('change', function () {
    handleImagePreview(this);
});

function createsubmitForm() {
    document.getElementById("post-form").submit();

    setTimeout(() => {
        clearForm();
    }, 100);
}

function clearForm() {
    document.getElementById("post-form").reset();
    window.location.href = "/home/index";
}