//da svaki button koji u sebi ima stil delete pita korisnika za potvrdu brisanja
$(function () {
    //$(".delete").click(function (event)){
    $(document).on('click', '.delete', function (event) {
        if (!confirm("Obrisati zapis?")) {
            event.preventDefault();
        }
    });
});

function clearOldMessage() {
    $('#tempmessage').siblings().remove();
    $('#tempmessage').removeClass("alert-success");
    $('#tempmessage').removeClass("alert-danger");
    $('#tempmessage').html('');
}

function SetDeleteAjax(selector, url, paramname) {
    $(document).on('click', selector, function (event) {
        event.preventDefault();
        var paramval = $(this).data(paramname);
        var tr = $(this).parents("tr");
        var aktivan = $(tr).data('aktivan');
        if (aktivan !== true) {
            $(tr).data('aktivan', true);
            if (confirm("Obrisati zapis?")) {
                var token = $('input[name="__RequestVerificationToken"]').first().val();
                clearOldMessage();
                $.post(url, { kontaktPon: paramval, __RequestVerificationToken : token }, function (data) {
                    if (data.successful) {
                        $(tr).remove();
                    }
                    $('#tempmessage').addClass(data.successful ? "alert-success" : "alert-danger");
                    $('#tempmessage').html(data.message);
                }).fail(function (jqXHR) { alert(jqXHR.status + " : " + jqXHR.responseText); $(tr).data('aktivan', false); })
            }
            else {
                $(tr).data('aktivan', false);
            }
        }
        
    });
    
}