﻿$(function () {
    $("a.searchadd").on("click", function (ev) {
        ev.preventDefault();
        $("<form id='search-add' class='modal fade hide validate ajax form-horizontal' data-width='700' data-keyboard='false' data-backdrop='static' />")
            .load($(this).attr("href"), {}, function () {
                $(this).modal("show");
                $.AttachFormElements();
                $(this).validate({
                    highlight: function (element) {
                        $(element).closest(".control-group").addClass("error");
                    },
                    unhighlight: function (element) {
                        $(element).closest(".control-group").removeClass("error");
                    }
                });
                $(this).on('hidden', function () {
                    $(this).remove();
                });
            });
    });
    $("#search-add a.commit").on("click", function (ev) {
        ev.preventDefault();
        var $this = $(this);
        var alreadyClicked = $this.data('clicked');
        if (alreadyClicked) {
            return false;
        }
        $this.data('clicked', true);
        var f = $this.closest("form");
        var q = f.serialize();
        var loc = $this.attr("href");
        $.post(loc, q, function (ret) {
            f.modal("hide");
            if (ret.message)
                swal("Error!", ret.message, "error");
            else if (ret.from === 'Menu')
                window.location = '/Person2/' + ret.pid;
            else
                AddSelected(ret);
        });
        return false;
    });
    $(document).on("shown", "#search-add", function () {
        $("#search-add #Name").focus();
        $("#search-add input").on("keydown", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                $("#searchperson").click();
                return false;
            }
            else
                return true;
        });
    });
    $.fn.loadWith = function (u, f) {
        var c = $(this);
        $.post(u, function (d) {
            c.replaceWith(d).ready(f);
        });
    };
    $("#search-add a.clear").on('click', function (ev) {
        ev.preventDefault();
        $("#Name").val('');
        $("#Phone").val('');
        $("#Address").val('');
        $("#dob").val('');
        return false;
    });

    $("form.ajax tbody > tr a.reveal").on("click", function (e) {
        e.stopPropagation();
    });
    $.NotReveal = function (ev) {
        if ($(ev.target).is("a"))
            if (!$(ev.target).is('.reveal'))
                return true;
        return false;
    };
    $("form.ajax tr.section").on("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        $ToggleShown($(this));
    });
    $('form.ajax a[rel="reveal"]').on("click", function (ev) {
        ev.preventDefault();
        $ToggleShown($(this).parents("tr"));
    });
    var $ToggleShown = function (tr) {
        if (tr.hasClass("notshown"))
            $ShowAll(tr);
        else if (tr.hasClass("shown"))
            $CollapseAll(tr);
        else
            tr.next("tr").find("div.collapse")
                .off('hidden')
                .on("hidden", function (e) { e.stopPropagation(); })
                .collapse("toggle");
    };
    var $ShowAll = function (tr) {
        tr.nextUntil("tr.section").find("div.collapse")
            .off('hidden')
            .on("hidden", function (e) { e.stopPropagation(); })
            .collapse("show");
        tr.removeClass("notshown").addClass("shown");
        tr.find("i").removeClass("fa-caret-right").addClass("fa-caret-down");
    };
    var $CollapseAll = function (tr) {
        tr.nextUntil("tr.section").find("div.collapse")
            .off("hidden")
            .on("hidden", function (e) { e.stopPropagation(); })
            .collapse('hide');
        tr.removeClass("shown").addClass("notshown");
        tr.find("i").removeClass("fa-caret-down").addClass("fa-caret-right");
    };
    $("form.ajax tr.master").on("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        $(this).next("tr").find("div.collapse")
            .off('hidden')
            .on("hidden", function (e) { e.stopPropagation(); })
            .collapse("toggle");
    });
    $("form.ajax tr.details").on("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        ev.stopPropagation();
        $(this).find("div.collapse")
            .off("hidden")
            .on("hidden", function (e) { e.stopPropagation(); })
            .collapse('hide');
    });
});