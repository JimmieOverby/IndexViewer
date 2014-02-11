$(document).ready(function () {
    $.indexViewer = $.indexViewer || {};
    $.indexViewer.autoComplete = $.indexViewer.autoComplete || {};

    var shouldTriggerCodeCompletion = function (enteredText, position) {
        var lastDotPosition = getIndexOfCurrentWorkingDot(enteredText, position);
        var stringBetweenLastDotAndCursor = enteredText.substr(lastDotPosition, (position - 1) - lastDotPosition);
        
        if (stringBetweenLastDotAndCursor.indexOf(" ") != -1){
            //We have not dotted
            return false;
        }

        var lengthOfSearchVariableName = "searchItem".length;
        var wordBeforeDot = enteredText.substr(lastDotPosition - lengthOfSearchVariableName, lengthOfSearchVariableName);

        return (wordBeforeDot === "searchItem");

    };

    var getIndexOfCurrentWorkingDot = function (enteredText, position) {
        var startPostion = position;
        var substringBeforeCursor = enteredText.substr(0, startPostion);
        var lastDotPosition = substringBeforeCursor.lastIndexOf(".");
        return lastDotPosition;
    };

    var extractTerm = function (term, positionOfCursor) {
        var stringBeforeCursor = term.substr(0, positionOfCursor);
        var indexOfLastDot = stringBeforeCursor.lastIndexOf(".");
        var termToSearchFor = term.substr((indexOfLastDot + 1), positionOfCursor - indexOfLastDot);
        return termToSearchFor;
    };

    function split(val) {
        return val.split(/,\s*/);
    };

    var replaceSearchTerm = function (originalText, selectedText, currentCaretPosition) {
        var positionOfWorkingDot = getIndexOfCurrentWorkingDot(originalText, currentCaretPosition);
        var stringBeforeWorkingDot = originalText.substr(0, positionOfWorkingDot + 1);
        var stringAfterSearchTerm = originalText.substr(currentCaretPosition, originalText.length - currentCaretPosition);
        var newSearchTerm = stringBeforeWorkingDot + selectedText + stringAfterSearchTerm;
        return newSearchTerm;
    };

    var getIconHtml = function (member) {
        if (member.type === "Method") {
            return "<img src='/sitecore modules/Shell/IndexViewer/img/methodIcon.png' width='10'/>";
        }
        if (member.type === "Property") {
            return "<img src='/sitecore modules/Shell/IndexViewer/img/propertyIcon.png' width='10'/>";
        }

        return "";
    }
            
    $("#WhereStatementBox").autocomplete({
        minLength: 0,
        source: function (request, response) {
            var availableTags = $.indexViewer.autoComplete.searchItemMembers;
            var positionOfCursor = $("#WhereStatementBox").caret();
            response($.ui.autocomplete.filter(availableTags, extractTerm(request.term, positionOfCursor)));
        },
        focus: function (event, ui) {
            var originalValue = event.target.value;
            var focussedValue = ui.item.value;
            var currentCaretPosition = $("#WhereStatementBox").caret();
            var newStringForBox = replaceSearchTerm(originalValue, focussedValue, currentCaretPosition);
            var target = $("#" + event.target.id);
            target.val(newStringForBox);
            event.preventDefault();
            return false;
        },
        select:function( event, ui ) {
            return false;
        },
        search: function(event,ui){
            var target = $("#" + event.target.id);
            var cursor_pos = target.caret();
            var shouldTriggerSearch = shouldTriggerCodeCompletion(event.target.value, cursor_pos);
            if (!shouldTriggerSearch) {
                return false;
            }
        },
        messages: {
            noResults: '',
            results: function () { }
        },
        open: function (event, ui) {
            var input = $(event.target),
                widget = input.autocomplete("widget"),
                style = $.extend(input.css([
                    "font",
                    "border-left",
                    "padding-left"
                ]), {
                    position: "absolute",
                    visibility: "hidden",
                    "padding-right": 0,
                    "border-right": 0,
                    "white-space": "pre"
                }),
                div = $("<div/>"),
                pos = {
                    my: "left top",
                    collision: "none"
                },
                offset = 50; // magic number to align the first letter

            widget.css("width", "");

            div
                .text(input.val().replace(/\S*$/, ""))
                .css(style)
                .insertAfter(input);
            offset = Math.min(
                Math.max(offset + div.width(), 0),
                input.width() - widget.width()
            );
            div.remove();

            pos.at = "left+" + offset + " bottom";
            input.autocomplete("option", "position", pos);

            widget.position($.extend({ of: input }, pos));
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<a>" + getIconHtml(item) + item.name + "</a>")
            .appendTo(ul);
    };


});