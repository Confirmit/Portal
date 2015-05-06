function copyToClipboard(content) {
	if (window.clipboardData && window.clipboardData.setData) {
		window.clipboardData.setData("Text", content);
		return true;
	}
	else {
		try {
			netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
		}
		catch (e) {
			return false;
		}

		var clipboard = Components.classes["@mozilla.org/widget/clipboard;1"].getService();
		if (clipboard) {
			clipboard = clipboard.QueryInterface(Components.interfaces.nsIClipboard);
		}

		var transferable = Components.classes["@mozilla.org/widget/transferable;1"].createInstance();
		if (transferable) {
			transferable = transferable.QueryInterface(Components.interfaces.nsITransferable);
		}

		if (clipboard && transferable) {
			var textObj = new Object();
			var textObj = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
			if (textObj) {
				textObj.data = content;
				transferable.setTransferData("text/unicode", textObj, content.length * 2);
				var clipid = Components.interfaces.nsIClipboard;
				clipboard.setData(transferable, null, clipid.kGlobalClipboard);

				return true;
			}
		}
		return false;
	}
}