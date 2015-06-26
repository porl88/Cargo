/*

DRAG AND DROP HTML5 EVENTS:
    dragstart	called when a drag item is first moved
    drag		called continuously whilst the drag item is being dragged
    dragenter	when set on the drop item, called whenever a dragged item is dragged over the drop item
    dragleave	when set on the drop item, called whenever a dragged item is dragged off the drop item
    dragover	called continuously whilst a drag item is being dragged over/hovering over a drop item
    drop		called when the drag item is dropped on a drop item
    dragend		called when the drag item is dropped, irrespective of whether or not it is over a drop item - called after the drop event

THE MINIMUM TO GET DRAG AND DROP TO WORK:

1. add dragstart event to drag item and use e.dataTransfer.setData() to capture the element
2. add dragover event to drop item and call preventDefault();
3. add drop event to drop item and call preventDefault() to stop the browser redirecting
*/



var DragAndDrop = (function () {
	'use strict';

	var dragAndDrop = {};
	var _onDrop, _onFilesDrop, _onFileDrop, _onError;
	var _errors;
	var _tempId = 'temp-zKsuPPv';

	dragAndDrop.enable = function () {
		var doc = document;

		if (validate()) {

			// DRAG ITEMS
			doc.body.addEventListener('dragstart', handleDragStart);
			doc.body.addEventListener('dragend', handleDragEnd);

			// DROP ZONE
			var dropItems = doc.querySelectorAll('[dropzone]');
			for (var i = 0, len = dropItems.length; i < len; i++) {
				var dropItem = dropItems[i];
				dropItem.addEventListener('dragenter', handleDragEnter);
				dropItem.addEventListener('dragleave', handleDragLeave);
				dropItem.addEventListener('dragover', handleDragOver);
				dropItem.addEventListener('drop', handleDrop); // called when the drag item is dropped on a drop item
			}
		}
	};

	dragAndDrop.onDrop = function (callback) {
		_onDrop = callback;
	};

	// returns all files, unvalidated
	dragAndDrop.onFilesDrop = function (callback) {
		_onFilesDrop = callback;
	};

	// called on each validated file
	dragAndDrop.onFileDrop = function (callback) {
		_onFileDrop = callback;
	};

	dragAndDrop.onError = function (callback) {
		_onError = callback;
	};

	function validate() {
		var docElement = document.documentElement;
		if (!supportsDragAndDrop()) return false;
		if (!docElement.classList) return false;
		return true;
	}

	function supportsDragAndDrop() {
		var div = document.createElement('div');
		return ('draggable' in div) || ('ondragstart' in div && 'ondrop' in div);
	}

	function reset() {
		_errors = [];
	}

	function getPermittedFileTypes(dropzone) {
		var permittedFileTypes = dropzone.match(/file:([a-z]+\/(\*|[a-zA-Z0-9+.-]+))/g);
		var fileTypeString = '';

		if (permittedFileTypes && permittedFileTypes.length > 0) {
			fileTypeString = ' Allowed file types are : ' + permittedFileTypes.join(', ').replace(/file:/g, '') + '.';
		}
		else {
			fileTypeString = ' Files dragged from the desktop are not allowed.';
		}

		return fileTypeString;
	}

	function getDropType(dropzone) {

		if (dropzone.indexOf('copy') > -1) {
			return 'copy';
		}
		else if (dropzone.indexOf('move') > -1) {
			return 'move';
		}

		return null;
	}

	function handleDragStart(e) { // in some browsers, e.g. Firefox, dragging won't be enabled until there is a drop object
		var dragItem = e.target;

		reset();

		var draggable = dragItem.getAttribute('draggable');
		if (draggable !== 'true') {
			return;
		}

		dragItem.classList.add('drag');

		// store drag item data
		if (!dragItem.id) {
			dragItem.id = _tempId;
		}

		e.dataTransfer.setData('text', dragItem.id);
	}

	function handleDragOver(e) {
		e.preventDefault(); // drag and drop will not work without prevenDefault() being called on the dragover event!!
	}

	function handleDragEnter(e) {
		e.preventDefault();
		e.stopPropagation();
		var dropItem = this;
		dropItem.classList.add('drag-over');
	}

	function handleDragLeave(e) {
		var dropItem = this;
		dropItem.classList.remove('drag-over');
	}

	function handleDrop(e) {
		e.preventDefault(); // stops some browsers from redirecting
		e.stopPropagation(); // stops some browsers from redirecting

		var doc = document;
		var dropItem = this;
		var dropzone = dropItem.getAttribute('dropzone');

		// capture files dragged from the desktop
		var files = e.dataTransfer.files;

		if (files && files.length) {

			if (_onFilesDrop) {
				_onFilesDrop(e, files);
			}

			reset();

			var fileTypeString = getPermittedFileTypes(dropzone);

			// process files
			for (var i = 0, len = files.length; i < len; i++) {
				var file = files[i];

				var isPermittedFileType = (dropzone.indexOf('file:' + file.type) > -1) || (dropzone.indexOf('file:' + file.type.match('[a-z]+/') + '*') > -1);

				if (file.type && isPermittedFileType) {
					if (_onFileDrop) {
						_onFileDrop(file);
					}
				}
				else {
					// log errpr
					_errors.push({
						fileName: file.name,
						message: 'This type of file is not permitted (' + file.type + ').' + fileTypeString
					});
				}
			}

		}
		else {
			// normal drag and drop

			// get drag item
			var dragItemId = e.dataTransfer.getData('text');
			if (!dragItemId) return;
			var dragItem = doc.getElementById(dragItemId);
			if (!dragItem) return;

			dragItem.classList.remove('drag');
			dropItem.classList.remove('drag-over');

			// work out type of drop
			var dropType = getDropType(dropzone);

			if (dragItemId === _tempId) {
				dragItem.removeAttribute('id');
			}

			if (_onDrop) {
				switch (dropType) {
					case 'copy':
						var clone = dragItem.cloneNode(true);
						clone.removeAttribute('id');
						_onDrop(e, clone, dropItem, dropType);
						break;
					case 'move':
						_onDrop(e, dragItem, dropItem, dropType);
						break;
				}
			}
		}

		// call error event
		var errors = _errors;
		if (errors.length > 0 && _onError) {
			_onError(errors);
		}
	}

	function handleDragEnd(e) {
		var dragItem = e.target;
		dragItem.classList.remove('drag');
		if (dragItem.id === _tempId) {
			dragItem.removeAttribute('id');
		}
	}

	return dragAndDrop;
}());

