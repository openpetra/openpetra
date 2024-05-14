// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2019-2024 by SolidCharity.com
//
// This file is part of OpenPetra.
//
// OpenPetra is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

class Modal {

    constructor() {
        $(document).on('show.bs.modal', '.modal', function (event) {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function() {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        });

        $(document).on('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
        });

        this.RemoveBackDropOnBrowserBack();
        this.modal_aquire = true;
        this.modal_timeout = 1;
    }

    // returns true of false,
    // after returning true, it will return false for the next X sec,
    // use this function for functions that open new modals, to prevent overlapping or load errors
    allow_modal() {
        if (!this.modal_aquire) {return false;}
        this.modal_aquire = false;
        window.setTimeout(function () { modal.reset_modal_timeout(); }, this.modal_timeout*1000);
        return true;
    }
    reset_modal_timeout() {
        this.modal_aquire = true
    }

    RemoveBackDropOnBrowserBack() {
            $(window).on('popstate', function (event) {  //pressed back button
                if(event.state!==null) {
                    $('.modal-backdrop').remove();
                }
            });
    }

    ShowModal(id, html) {
        // clear previous instance of this modal
        $('#modal_space #'+id).remove();
        // set the id for this modal
        $(html).attr('id', id);
        // insert the new modal
        $('#modal_space').append(html);
        // get the reference to this new modal
        let m = $('#modal_space #'+id);
        // make sure that we cannot close this modal by clicking outside
        m.modal({backdrop:"static", keyboard: false});
        // show the modal
        m.modal('show');
        return m;
    }

    FindModal(id) {
        return $('#modal_space #'+id);
    }

    FindMyModal(obj) {
        return $(obj.closest(".modal"))
    }

    // close the modal closest to the button, which is eg. a Save/Delete/Close button
    // but we can pass the modal element itself as well
    CloseModal(btn, remove=true) {
        let m=$(btn).closest('.modal');
        m.modal('hide');
        if (remove) {
            window.setTimeout(function() {m.remove();}, 500);
        }
    }
}

let modal = new Modal()
export default modal