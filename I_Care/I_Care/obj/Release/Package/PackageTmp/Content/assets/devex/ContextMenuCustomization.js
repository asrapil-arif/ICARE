(function() {
    function RichEditPopupMenuShowing(s, e) {
        if(!chbShowContextMenu.GetChecked())
            e.cancel = true;
        else {
            if(!chbShowDefaultItems.GetChecked())
                e.menuItems.Clear();

            var insertDateItem = new ASPxClientRichEditPopupMenuItem();
            insertDateItem.name = "addDate";
            insertDateItem.text = "Insert the Current Date";
            e.menuItems.Insert(0, insertDateItem);

            var insertTimeItem = new ASPxClientRichEditPopupMenuItem();
            insertTimeItem.name = "addTime";
            insertTimeItem.text = "Insert the Current Time";
            e.menuItems.Insert(1, insertTimeItem);

            var googleSearchItem = new ASPxClientRichEditPopupMenuItem();
            googleSearchItem.name = "googleSearch";
            googleSearchItem.text = "Google Search...";
            googleSearchItem.imageUrl = "../Content/search.png";
            googleSearchItem.beginGroup = true;
            e.menuItems.Insert(2, googleSearchItem);
        }
    }

    function RichEditCustomCommandExecuted(s, e) {
        switch(e.commandName) {
            case "googleSearch":
                var selectedInterval = s.selection.intervals[0];
                var selectedText = s.document.activeSubDocument.text.substr(selectedInterval.start,
                    selectedInterval.length);
                window.open("https://www.google.com/search?q=" + selectedText, "_blank");
                break;
            case "addDate":
                s.commands.createDateField.execute();
                break;
            case "addTime":
                s.commands.createTimeField.execute();
                break;
        }
    }

    window.RichEditPopupMenuShowing = RichEditPopupMenuShowing;
    window.RichEditCustomCommandExecuted = RichEditCustomCommandExecuted;
})();
