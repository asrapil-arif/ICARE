(function() {
    var commandsTable = {
        "bold": function (rich) { return rich.commands.changeFontBold },
        "italic": function (rich) { return rich.commands.changeFontItalic },
        "underline": function (rich) { return rich.commands.changeFontUnderline },
        "strikeout": function (rich) { return rich.commands.changeFontStrikeout },
        "alignleft": function (rich) { return rich.commands.toggleParagraphAlignmentLeft },
        "aligncenter": function (rich) { return rich.commands.toggleParagraphAlignmentCenter },
        "alignright": function (rich) { return rich.commands.toggleParagraphAlignmentRight },
        "listbullets": function (rich) { return rich.commands.toggleBulletedList },
        "listnumbers": function (rich) { return rich.commands.toggleNumberingList },
        "insertlink": function (rich) { return rich.commands.openInsertHyperlinkDialog },
        "insertpicture": function (rich) { return rich.commands.openInsertPictureDialog }
    };

    function onToolbarItemClick(s, e) {
        var command = commandsTable[e.item.name].call(this, DemoRichEdit);
        command.execute();
    }
    function updateToolbar() {
        for (var key in commandsTable) {
            if (!commandsTable.hasOwnProperty(key)) continue;
            updateToolbarItem(key, commandsTable[key].call(this, DemoRichEdit));
        }
    }
    function updateToolbarItem(itemName, command) {
        var item = DemoToolbar.GetItemByName(itemName);
        var state = command.getState();
        item.SetEnabled(state.enabled);
        item.SetChecked(state.value);
    }

    window.onToolbarItemClick = onToolbarItemClick;
    window.updateToolbar = updateToolbar;
})();
