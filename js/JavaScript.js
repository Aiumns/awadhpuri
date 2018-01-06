var source, destination, displayPath;
var directionsService = new google.maps.DirectionsService();
google.maps.event.addDomListener(window, 'load', function () {
    new google.maps.places.SearchBox(document.getElementById('inputSource'));
    new google.maps.places.SearchBox(document.getElementById('inputDestination'));
    displayPath = new google.maps.DirectionsRenderer({ 'draggable': true });
});
function SearchPath() {
    var m2kCinema = new google.maps.LatLng(28.7009364, 77.1166042);
    var mapOptions = {
        zoom: 16,
        center: m2kCinema
    };
    map = new google.maps.Map(document.getElementById('showMap'), mapOptions);
    displayPath.setMap(map);

    source = document.getElementById("inputSource").value;
    destination = document.getElementById("inputDestination").value;

    var request = {
        origin: source,
        destination: destination,
        travelMode: google.maps.TravelMode.DRIVING
    };

    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            displayPath.setDirections(response);
        }
    });
    return false;
}
