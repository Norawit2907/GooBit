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
            var defaultCoordinates = { lat: oldlatitude, lng: oldlongitude };
            map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: oldlatitude, lng: oldlongitude },
                zoom: 5
            });

            var defaultMarkerPosition = new google.maps.LatLng(defaultCoordinates.lat, defaultCoordinates.lng);
            latitude = 13;
            longitude = 100;

            // Add a marker on the map
            marker = new google.maps.Marker({
                map: map,
                position: defaultMarkerPosition,
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
    document.getElementById("edit-form").reset();
    const previewContainer = document.getElementById('image-preview');
    previewContainer.innerHTML = '';
}

function editsubmitForm() {
    document.getElementById("tags").value = document.querySelector(".tags_input").value;
    document.getElementById("edit-form").submit();
    setTimeout(() => {
        // const formData = new FormData(document.getElementById("edit-form"));
        // for (const [key, value] of formData.entries()) {
        //     console.log(`${key}: ${value}`);
        // }
        clearForm();
    }, 1000); 
}

document.addEventListener("DOMContentLoaded", function(){
    const customSelects = document.querySelectorAll(".custom-select");
    function updateSelectedOptions(customSelect){
        const selectedOptions = Array.from(customSelect.querySelectorAll(".option.active")).filter(option =>
            option !== customSelect.querySelector(".option.all-tags")).map(function(option){
                return {
                    value: option.getAttribute("data-value"),
                    text: option.textContent.trim()
                };
            });

            const maxTagsLimit = availableUser;
            if (selectedOptions.length > maxTagsLimit) {
                alert('You cannot select more.');
                const lastSelectedOption = selectedOptions[selectedOptions.length - 1];
                const lastSelectedOptionElement = customSelect.querySelector(`.option[data-value="${lastSelectedOption.value}"]`);
                lastSelectedOptionElement.classList.remove('active');
                updateSelectedOptions(customSelect); 
                return;
            }
        
            const selectedValues = selectedOptions.map(function(option){
                return option.value;
            });

            customSelect.querySelector(".tags_input").value = selectedValues.join(',');
            
            let tagsHTML = "";

            if(selectedOptions.length === 0){
                tagsHTML = '<span class="placeholder">Choose People</span>';
            }
            else{
                const maxTagsToShow = 3;
                let additionalTagsCount = 0;

                selectedOptions.forEach(function(option, index){
                    if(index < maxTagsToShow){
                        tagsHTML += '<span class="tag">'+option.text +'<span class="remove-tag" data-value="'+option.value+'">&times;</span></span>';
                    }
                    else{
                        additionalTagsCount++;
                    }
                });

                if(additionalTagsCount > 0){
                    tagsHTML += '<span class="tag">+' +additionalTagsCount+'</span>';
                }
            }
            customSelect.querySelector(".selected-options").innerHTML = tagsHTML;
    }

    customSelects.forEach(function(customSelect){
        const searchInput = customSelect.querySelector(".search-tags");
        const optionContainer = customSelect.querySelector(".options");
        const noResultMatch = customSelect.querySelector(".no-result-match");
        const options = customSelect.querySelectorAll(".option");
        const allTagsOption = customSelect.querySelector(".option.all-tags");
        const clearButton = customSelect.querySelector(".clear");

        allTagsOption.addEventListener("click", function(){
            const isActive = allTagsOption.classList.contains("active");
            options.forEach(function(option){
                if(option !== allTagsOption){
                    option.classList.toggle("active", !isActive);
                }
            });
            updateSelectedOptions(customSelect);
        });

        clearButton.addEventListener("click", function(){
            searchInput.value = "";
            options.forEach(function(option){
                option.style.display = "block";
            });
            noResultMatch.style.display = "none";
        });

        searchInput.addEventListener("input", function(){
            const searchTerm = searchInput.value.toLowerCase();

            options.forEach(function(option){
                const optionText = option.textContent.trim().toLowerCase();
                const shouldShow = optionText.includes(searchTerm);
                option.style.display = shouldShow ? "block" : "none";
            });
                
            const anyOptionsMatch = Array.from(options).some
            (option => option.style.display === "block");
            noResultMatch.style.display = anyOptionsMatch ? "none" : "block";

            if(searchTerm){
                optionContainer.classList.add("option-search-active");
            }
            else{
                optionContainer.classList.remove("option-search-active");
            }
        });
    });

    customSelects.forEach(function(customSelect){
        const options = customSelect.querySelectorAll(".option");
        options.forEach(function(option){
            const optionValue = option.getAttribute("data-value");
            
            if(optionValue !== "ALL" &&submitted_users.some(user => user.id === optionValue)){
                option.classList.toggle("active");
            }
            option.addEventListener("click", function(){
                option.classList.toggle("active");
                updateSelectedOptions(customSelect);
            });
        });
    });

    document.addEventListener("click", function(event){
        const removeTag = event.target.closest(".remove-tag");
        if(removeTag){
            console.log("Remove tag clicked");
            const customSelect = removeTag.closest(".custom-select");
            const valueToRemove = removeTag.getAttribute("data-value");
            console.log("Value to remove: " + valueToRemove);
            const optionToRemove = customSelect.querySelector(".option[data-value='"+valueToRemove+"']");
            optionToRemove.classList.remove("active");

            // const otherSelectionOptions = customSelect.querySelectorAll(".option.active:not(.all-tags)");
            // const allTagsOption = customSelect.querySelector("option.all-tags");

            // // if(otherSelectionOptions.length === 0){
            // //     allTagsOption.classList.remove("active");
            // // }
            updateSelectedOptions(customSelect);
        }
    });

    const selectBoxes = document.querySelectorAll(".select-box");
    selectBoxes.forEach(function(selectBox){
        selectBox.addEventListener("click",function(event){
            if(!event.target.closest(".tag")){
                selectBox.parentNode.classList.toggle("open");
            }
        });
    });

    document.addEventListener("click",function(event){
        if(!event.target.closest(".custom-select") && !event.target.classList.contains("remove-tag")){
            customSelects.forEach(function(customSelect){
                customSelect.classList.remove("open");
            });
        }
    });

    function resetCustomSelects(){
        customSelects.forEach(function(customSelect){
            customSelect.querySelectorAll(".option.active").forEach(function(option){
                option.classList.remove("active");
            });
            customSelect.querySelector("option.all-tags").classList.remove("active");
            updateSelectedOptions(customSelect);
        });
    }
    updateSelectedOptions(customSelects[0]);

    // const editsubmitButton = document.querySelector(".editsubmitBtn");
    // editsubmitButton.addEventListener("click", function(){
    //     customSelects.forEach(function(customSelect){
    //         const selectedOptions = customSelect.querySelectorAll(".option.active");
    //     }); 
    //     resetCustomSelects();
    // })
});