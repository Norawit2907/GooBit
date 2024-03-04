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

let map, marker, latitude, longitude;

        function initMap() {
            map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 13, lng: 100 },
                zoom: 5
            });

            latitude = 13;
            longitude = 100;

            // Add a marker on the map
            marker = new google.maps.Marker({
                map: map,
                draggable: true
            });

            // Listen for the marker dragend event to update the selected location
            google.maps.event.addListener(marker, 'dragend', function () {
                updateSelectedLocation(marker.getPosition());
            });

            // Add a Places Autocomplete input field
            const input = document.getElementById('locationInput');
            const autocomplete = new google.maps.places.Autocomplete(input);

            // Listen for the place changed event to update the map and selected location
            autocomplete.addListener('place_changed', function () {
                const place = autocomplete.getPlace();

                if (!place.geometry) {
                    alert("No details available for input: '" + place.name + "'");
                    return;
                }

                // Update the map and selected location
                map.setCenter(place.geometry.location);
                marker.setPosition(place.geometry.location);
                updateSelectedLocation(place.geometry.location);
            });
        }

        function updateSelectedLocation(location) {
            // Update the hidden input with the selected location
            document.getElementById('selectedLocation').value = JSON.stringify({
                lat: location.lat(),
                lng: location.lng()
            });
            latitude = location.lat()
            longitude = location.lng()
        }

        // Initialize the map when the page is loaded
        document.addEventListener('DOMContentLoaded', function () {
            initMap();
        });

function clearForm() {
    document.getElementById("post-form").reset();
}



$(document).ready(function() {
    $('#submitBtn').click(function() {
        var formData = new FormData();
        // Add form fields to FormData object
        formData.append("title", $("#title").val());
        formData.append("description", $("#description").val());
        formData.append("max_member", $("#max_member").val());
        formData.append("end_date", $("#end_date").val());
        formData.append("event_date", $("#event_date").val());
        formData.append("duration", $("#duration").val());
        formData.append("googlemap_location", $("#selectedLocation").val());
        formData.append("latitude", latitude);
        formData.append("longitude", longitude);
        formData.append('category', $("#category").val());
        
        var totalImage = $('#image-input')[0].files.length;
        console.log(totalImage);
        console.log("tcsa")
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
    
