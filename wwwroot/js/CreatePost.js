document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.getElementById('image-input');
    const selectedImagesContainer = document.getElementById('selected-images');

    // Check if there are previously selected images in local storage
    const storedImages = JSON.parse(localStorage.getItem('selectedImages')) || [];

    // Display previously selected images
    storedImages.forEach(imageData => {
        displaySelectedImage(imageData);
    });

    fileInput.addEventListener('change', handleFileSelect);

    function handleFileSelect(event) {
        const files = event.target.files;
        for (const file of files) {
            const reader = new FileReader();

            reader.onload = function (e) {
                const imageData = {
                    name: file.name,
                    dataUrl: e.target.result
                };

                displaySelectedImage(imageData);

                // Store the selected image data in local storage
                // storedImages.push(imageData);
                // localStorage.setItem('selectedImages', JSON.stringify(storedImages));
            };

            reader.readAsDataURL(file);
        }
    }

    function displaySelectedImage(imageData) {
        const imgContainer = document.createElement('div');
        imgContainer.classList.add('selected-image-container');

        const imgElement = document.createElement('img');
        imgElement.src = imageData.dataUrl;
        imgElement.alt = imageData.name;
        imgElement.classList.add('selected-image');
        imgContainer.appendChild(imgElement);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Remove';
        deleteButton.classList.add('delete-button');
        deleteButton.addEventListener('click', function () {
            removeSelectedImage(imgContainer, imageData);
        });
        imgContainer.appendChild(deleteButton);

        selectedImagesContainer.appendChild(imgContainer);
    }

    function removeSelectedImage(container, imageData) {
        // Remove the image container from the display
        container.remove();

        // Remove the image data from the stored images
        const index = storedImages.findIndex(img => img.name === imageData.name);
        if (index !== -1) {
            storedImages.splice(index, 1);
            localStorage.setItem('selectedImages', JSON.stringify(storedImages));
        }
    }
});

let selectedCategory = null;
function updateButtonText(option) {
    document.getElementById('selectedOption').innerText = option;
    selectedCategory = option;
}

var map;
var marker;

function initMap() {
    var center = { lat: 40.7128, lng: -74.0060 };
    map = new google.maps.Map(
        document.getElementById('map'), { zoom: 10, center: center });
}

let latitude, longitude;

function searchLocation() {
    var locationName = document.getElementById('googlemap_location').value;
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': locationName }, function (results, status) {
        if (status === 'OK') {
            latitude = results[0].geometry.location.lat();
            longitude = results[0].geometry.location.lng();
            var center = { lat: latitude, lng: longitude };
            map.setCenter(center);
            if (marker) {
                marker.setMap(null);
            }
            marker = new google.maps.Marker({ position: center, map: map });
        } else {
            console.error('Geocode was not successful for the following reason: ' + status);
        }
    });
}

function loadMapScript() {
    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyBuU9Wcj-4Z3ikPd9dp75Z8Hxdu3WII9Wc&callback=initMap';
    document.head.appendChild(script);
}

loadMapScript();

function chooseCategory(){

}


$(document).ready(function() {
    $('#createButton').click(function() {
        var formData = new FormData();
        // Add form fields to FormData object
        formData.append("title", $("#title").val());
        formData.append("description", $("#description").val());
        formData.append("max_member", $("#max_member").val());
        formData.append("end_date", $("#end_date").val());
        formData.append("event_date", $("#event_date").val());
        formData.append("duration", $("#duration").val());
        formData.append("googlemap_location", $("#googlemap_location").val());
        formData.append("latitude", latitude);
        formData.append("longitude", longitude);
        formData.append('category', selectedCategory);
        
        var totalImage = $('#image-input')[0].files.length;
        console.log(totalImage);
        for (var i = 0; i < totalImage; i++) {
            var image = $('#image-input')[0].files[i];
            formData.append("images", image);
        }
        
        $.ajax({
            type: 'POST',
            url: '/event/create',
            dataType: 'json',
            data: formData,
            contentType: false,
            processData: false,
            success: function(response) {
                alert('Upload successful' + response);
                // Handle response
            },
            error: function (ts) { alert(ts.responseText) } // error where is layout?
        });
    });
    
});
    
