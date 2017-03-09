$(document).ready(function () {
    $("#medlemmars").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Calendar/Index",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name , value: item.Name };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
})