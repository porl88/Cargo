var Cargo = window.Cargo || {};

Cargo.ColumnMapping = (function () {
    'use strict';

    // PUBLIC
    return {
        init: function () {
            var doc = document;
            doc.addEventListener('DOMContentLoaded', function () {

                var dragAndDrop = DragAndDrop;

                dragAndDrop.enable();

                dragAndDrop.onDrop(function (e, dragItem, dropItem, dropType) {
                    dropItem.appendChild(dragItem);
                    var field = dragItem.querySelector('input');
                    if (field) {
                        field.value = dropItem.dataset.column;
                    }

                    return true;
                });
            });

        }
    }

})();



