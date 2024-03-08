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
    document.getElementById("edit-form").reset();
}

function editsubmitForm() {
    document.getElementById("tags").value = document.querySelector(".tags_input").value;
    // document.getElementById("edit-form").submit();
    const formData = new FormData(document.getElementById("edit-form"));
    for (const [key, value] of formData.entries()) {
        console.log(`${key}: ${value}`);
    }
    setTimeout(function() {
        document.getElementById("edit-form").reset();
    }, 100);
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

            const maxTagsLimit = 5;
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
