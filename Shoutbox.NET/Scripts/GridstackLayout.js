var loadGridLayout = function(serializedLayout) {
        for (var i = 0; i < serializedLayout.length; i++) {
        $("#" + serializedLayout[i]["id"]).attr("data-gs-width", serializedLayout[i]["width"]);
        $("#" + serializedLayout[i]["id"]).attr("data-gs-height", serializedLayout[i]["height"]);
        $("#" + serializedLayout[i]["id"]).attr("data-gs-x", serializedLayout[i]["x"]);
        $("#" + serializedLayout[i]["id"]).attr("data-gs-y", serializedLayout[i]["y"]);
    }
}

var saveGridLayout = function() {
    var res = _.map($('.grid-stack .grid-stack-item:visible'), function(el) {
        el = $(el);
        var node = el.data('_gridstack_node');
        return {
            id: el.attr('id'),
            x: node.x,
            y: node.y,
            width: node.width,
            height: node.height
        };
    });
    return (JSON.stringify(res));
}

$(document).ready(function () {
    $('.grid-stack').on('change', function (event, items) {
        var newLayout = saveGridLayout();
        sendUpdatedGridLayout(newLayout);
    });
});