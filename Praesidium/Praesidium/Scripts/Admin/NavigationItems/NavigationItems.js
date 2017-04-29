$(document).on("click", "#editModal", function () {
    var Controller = $(this).data('controller');
    var Action = $(this).data('action');
    var DisplayText = $(this).data('displaytext');
    var Section = $(this).data('section');
    var Active = $(this).data('active');

    $(".modal-body #controller").val(Controller);
    $(".modal-body #action").val(Action);
    $(".modal-body #displayText").val(DisplayText);
    $(".modal-body #ddlSections").val(Section);
    debugger;
    if (Active == "True"){
        $(".modal-body #active").val(true);
    } else {
        $(".modal-body #active").val(false);
    }
    
});