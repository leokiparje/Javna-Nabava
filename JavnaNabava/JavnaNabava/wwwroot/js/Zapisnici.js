/*
$(zapisnik).on('click', '.deleterow', function () {
    event.preventDefault();
    var tr = $(this).parents("tr");
    tr.remove();
    clearOldMessage();
});
*/
$(function () {
    $(".form-control").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
        }
    });

    $("#odredba-dodaj").click(function () {
        event.preventDefault();
        dodajStavku();
    });
});

function dodajStavku() {
    var id = $("#odredba-id").val();
    if (id != '') {
        if ($("[name='StavkeZapisnika[" + id + "].idOdredba'").length > 0) {
            alert('Odredba je već u dokumentu');
            return;
        }

        var template = $('#template').html();
        var naziv = $("#odredba-naziv").val();
        var cijena = $("#odredba-cijena").val();
        var ispravnost = $("#odredba-ispravnost").val();

        if ("" + ispravnost.localeCompare("valjan") != 0 && "" + ispravnost.localeCompare("nevaljan")!=0) {
            alert("Valjanost odredbe može poprimiti vrijednosti : valjan ili nevaljan");
            return;
        }

        //Alternativa ako su hr postavke sa zarezom //http://haacked.com/archive/2011/03/19/fixing-binding-to-decimals.aspx/
        //ili ovo http://intellitect.com/custom-model-binding-in-asp-net-core-1-0/

        template = template.replace(/--id--/g, id)
            .replace(/--naziv--/g, naziv)
            .replace(/--cijena--/g, cijena)
            .replace(/--ispravnost--/g, ispravnost)
        $(template).find('tr').insertBefore($("#table-stavke").find('tr').last());

        $("#odredba-id").val('');
        $("#odredba-naziv").val('');
        $("#odredba-cijena").val('');
        $("#odredba-ispravnost").val('');

        clearOldMessage();
    }
}