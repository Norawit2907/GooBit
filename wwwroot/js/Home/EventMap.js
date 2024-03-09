
    // Initialize and add the map
    function initbannerMap() {
        var center = { lat: 40.7128, lng: -74.0060 };
        var bannermap = new google.maps.Map(document.getElementById('bannermap'), { zoom: 10, center: center });
        
        var locationName = document.getElementById('location').innerHTML;
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': locationName }, function(results, status) {
            if (status === 'OK') {
            var latitude = results[0].geometry.location.lat();
            var longitude = results[0].geometry.location.lng();
            var center = { lat: latitude, lng: longitude };
            bannermap.setCenter(center);
            var bannermarker = new google.maps.Marker({ position: center, map: bannermap });
            } else {
            console.error('Geocode was not successful for the following reason: ' + status);
            }
        });  
    }


    function initPageMap() {
        var center = { lat: 40.7128, lng: -74.0060 };
        var bannermap = new google.maps.Map(document.getElementById('pagemap'), { zoom: 10, center: center });
        
        var locationName = document.getElementById('location').innerHTML;
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': locationName }, function(results, status) {
            if (status === 'OK') {
            var latitude = results[0].geometry.location.lat();
            var longitude = results[0].geometry.location.lng();
            var center = { lat: latitude, lng: longitude };
            bannermap.setCenter(center);
            var bannermarker = new google.maps.Marker({ position: center, map: bannermap });
            } else {
            console.error('Geocode was not successful for the following reason: ' + status);
            }
        });  
    }
    google.maps.event.addDomListener(window, "load", initbannerMap);
    google.maps.event.addDomListener(window, "load", initPageMap);




    
