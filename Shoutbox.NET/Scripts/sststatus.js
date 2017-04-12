var getStatusIcon = function (status) {
    switch (status) {
        case "Online":
            return '<i class="fa fa-check-circle" aria-hidden="true" title="Online" style="color:green"></i> Online';
        case "Warning":
            return '<i class="fa fa-exclamation-triangle" aria-hidden="true" title="Let op, vertraging" style="color:orange"></i> Opgelet!';
        case "Offline":
            return '<i class="fa fa-minus-circle" aria-hidden="true" title="Offline" style="color:red"></i> Offline';
        case "Unknown":
            return '<i class="fa fa-question-circle" aria-hidden="true" title="Onbekend" style="color:gray"></i> Onbekend';
    }
}

var setServiceStatus = function (service) {

    $.ajax({
        url: '/Status/' + service,
        contentType: "application/text charset=utf-8",
        dataType: "text",
        success: function (data) {
            $("#" + service).html(getStatusIcon(data));
        },
        error: function (data) {
            $("#" + service).html(getStatusIcon(data));
        }
    });

}