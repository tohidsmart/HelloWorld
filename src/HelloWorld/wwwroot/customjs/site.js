// site.js
(function () {

  var $sidebarAndWrapper = $("#sidebar,#wrapper");
  var $sidebarIcon=$("#sidebarToggle i.fa")

  $("#sidebarToggle").on("click", function () {
    $sidebarAndWrapper.toggleClass("hide-sidebar");
    if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
        $sidebarIcon.removeClass("fa-angle-left")
        $sidebarIcon.addClass("fa-angle-right");
    } else {
        $sidebarIcon.removeClass("fa-angle-right")
        $sidebarIcon.addClass("fa-angle-left");
    }
  });
})();