function handleImagePreview(input) {
    const previewContainer = document.getElementById('image-preview');
    previewContainer.innerHTML = '';

    const files = input.files;

    if (files) {
        Array.from(files).forEach(file => {
            const reader = new FileReader();

            reader.onload = function (e) {
                const image = document.createElement('img');
                image.src = e.target.result;
                image.classList.add('preview-image');
                previewContainer.appendChild(image);
            };

            reader.readAsDataURL(file);
        });
    }
}

document.getElementById('image-input').addEventListener('change', function () {
    handleImagePreview(this);
});

let map, marker;

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
            document.getElementById('latitude').value = location.lat()
            document.getElementById('longitude').value = location.lng()
        }

        // Initialize the map when the page is loaded
        document.addEventListener('DOMContentLoaded', function () {
            initMap();
        });

function clearForm() {
    document.getElementById("post-form").reset();
    const previewContainer = document.getElementById('image-preview');
    previewContainer.innerHTML = '';
}

function createsubmitForm() {
    document.getElementById("post-form").submit();

    setTimeout(function() {
        document.getElementById("post-form").reset();
    }, 100);
}