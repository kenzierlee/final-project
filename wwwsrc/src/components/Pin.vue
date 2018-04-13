<template>
    <div class="pin">
        <div class="card text-center">
            <div class="pin-base">
                <img class="card-img-top" :src="pin.imgUrl" alt="Pin Image">
                <div class="save">
                    <i class="fas fa-plus" @click="pinViews()" data-toggle="modal" :data-target="'#'+pin.id"></i>
                </div>
                <!-- </div> -->
                <footer class="footer">
                    <small class="text-muted">
                        <i class="fas fa-eye p-3"></i>{{pin.views}}
                        <i class="far fa-save p-3"></i>{{pin.saves}}
                        <div v-if="pin.userId==user.id" class="flexor">
                            <div v-if="pin.public==1">
                                <i class="fas fa-unlock pointer m-3" data-toggle="modal" :data-target="'#edit-'+pin.id"></i>
                            </div>
                            <div v-if="this.$route.boardId!==undefined">
                                <i class="fas fa-trash-alt pointer m-3" @click="deleteBoardPin(pin)"></i>
                            </div>
                            <div v-else>
                                <i class="fas fa-trash-alt pointer m-3" @click="deletePin(pin)"></i>
                            </div>
                        </div>
                    </small>
                </footer>
            </div>
        </div>
        <!-- EXPANDED PIN MODAL -->
        <div class="modal fade" :id="pin.id" tabindex="-1" role="dialog" aria-labelledby="Expanded-Pin" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <img class="modal-img" :src="pin.imgUrl" alt="Pin Image">
                    </div>
                    <div class="modal-body text-center">
                        <h5>{{pin.name}}</h5>
                        <p>{{pin.description}}</p>
                        <div class="dropdown">
                            <a class="btn btn-secondary dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true"
                                aria-expanded="false">
                                <i class="far fa-clipboard"></i> Board to Save to:
                            </a>
                            <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                <div v-for="board in boards">
                                    <a class="dropdown-item" @click="boardId = board.id">{{board.name}}</a>
                                </div>
                            </div>
                        </div>
                        <b>OR</b>
                        <div class="m-2">
                            <button type="button" class="btn btn-primary text-center" @click="modalSwitch()">
                                <i class="fas fa-plus"></i> Create New Board</button>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-sm grey-btn" data-dismiss="modal">
                            <b>Close</b>
                        </button>
                        <button @click="savePin()" class="btn btn-sm blue-btn">
                            <i class="fas fa-thumbtack"></i> Save Pin
                        </button>
                        <a :href="pin.href" target="_blank" class="btn btn-primary">Link to Source</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- /EXPAND PIN MODAL -->
        <!-- EDIT PIN MODAL -->
        <div class="modal fade" :id="'edit-'+pin.id" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content full-width">
                    <img class="mid" src="../assets/main-logo.png" height="150" width="180">
                    <form @submit.prevent="editPin(pin)">
                        <div class="modal-body">
                            <div class="m-2">
                                <i class="far fa-image fa-1x"></i>
                                <input type="text" name="pinUrl" v-model="pin.imgUrl" placeholder="Image URL" required>
                            </div>
                            <div class="m-2">
                                <i class="fas fa-external-link-alt fa-1x"></i>
                                <input type="text" name="pinHref" v-model="pin.href" placeholder="Website URL">
                            </div>
                            <div class="m-2">
                                <i class="far fa-1x fa-edit"></i>
                                <input type="text" name="pinName" v-model="pin.name" placeholder="Pin Name">
                            </div>
                            <div class="m-2">
                                <i class="far fa-1x fa-edit"></i>
                                <input type="text" name="pinDescription" v-model="pin.description" placeholder="Pin Description">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-sm grey-btn" data-dismiss="modal">
                                <b>Close</b>
                            </button>
                            <button type="submit" class="btn btn-sm blue-btn">
                                <b>Edit Pin</b>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- /EDIT PIN MODAL -->
        <!-- ADD BOARD -->
        <div class="modal fade" :id="'addBoard-'+pin.id" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content full-width">
                    <img class="mid" src="../assets/main-logo.png" height="150" width="180">
                    <form @submit.prevent="createBoard(board)">
                        <div class="modal-body">
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-edit"></i>
                                <input type="text" name="boardName" v-model="board.name" placeholder="Board Name">
                            </div>
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-edit"></i>
                                <input type="text" name="boardDescription" v-model="board.description" placeholder="Board Description">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-sm grey-btn" data-dismiss="modal">
                                <b>Close</b>
                            </button>
                            <button type="submit" class="btn btn-sm blue-btn">
                                <b>Add Board</b>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- /ADD BOARD -->
    </div>
</template>
<script>
    export default {
        name: 'Pins',
        props: ['pin'],
        data() {
            return {
                board: {},
                boardId: ""
            }
        },
        mounted() {
            this.$store.dispatch("getPins");
            this.$store.dispatch("getBoards");
            this.$store.dispatch("authenticate");
        },
        methods: {
            pinViews() {
                this.$store.dispatch("updateViews", this.pin.id)
            },
            savePin() {
                this.$store.dispatch("savePin", { pinId: this.pin.id, boardId: this.boardId });
                $('#' + this.pin.id).modal('hide');
            },
            editPin(pin) {
                this.$store.dispatch("editPin", pin);
                $('#edit-' + pin.id).modal('hide');
            },
            deletePin(pin) {
                this.$store.dispatch("deletePin", pin.id);
            },
            deleteBoardPin(pin) {
                this.$store.dispatch("deleteBoardPin", pin);
            },
            modalSwith() {
                $('#' + this.pin.id).modal('hide');
                $('#addBoard-' + this.pin.id).modal('show');
            },
            createBoard() {
                this.$store.dispatch('createBoard', this.board);
                $('#addBoard-' + this.pin.id).modal('show');
                $('#' + this.pin.id).modal('show');
                this.notifyBoard();
                this.board = {};
            },
            notifyBoard() {
                alertify.set('notifier', 'position', 'bottom-center');
                alertify.notify('Board Added!', 'success', 5, function () { console.log('dismissed'); });
            }
        },
        computed: {
            pins() {
                return this.$store.state.pins;
            },
            boards() {
                return this.$store.state.boards;
            },
            user() {
                return this.$store.state.user;
            }
        }
    }
</script>
<style scoped>
    .dropdown-item:hover {
        cursor: pointer;
        background-color: rgb(192, 192, 192);
    }

    .modal-img {
        max-width: 38vw;
    }

    .pin-base {
        position: relative;
        display: inline-block;
    }

    .pin-base:hover .save {
        display: block;
        cursor: pointer;
    }

    .save {
        padding-top: 7px;
        padding-right: 7px;
        position: absolute;
        right: 0;
        top: 0;
        display: none;
    }

    .save a {
        color: #000;
    }

    .pointer:hover {
        cursor: pointer;
    }

    .full-width {
        width: 100%;
    }

    .blue-btn {
        background-color: #95BAC3;
        color: white;
        transition: all .3s linear;
    }

    .blue-btn:hover {
        background-color: #5d8dcf;
        color: white;
    }

    .grey-btn {
        background-color: #afafaf;
        color: white;
        transition: all .3s linear;
    }

    .grey-btn:hover {
        background-color: #7a7a7a;
        color: white;
    }

    .mid {
        margin: .5rem auto;
    }

    input {
        width: 95%;
    }

    a {
        color: #fff;
    }

    a:hover {
        text-decoration: none
    }

    .pointer {
        cursor: pointer;
    }

    .ddcss {
        color: black;
        background-color: white;
        font-style: normal;
    }

    .dropdown-menu {
        min-width: 0;
        padding: 1rem 0;
        margin-right: 10px;
        left: -8rem;
        z-index: 1021;
    }

    .dropdown-item:hover {
        background-color: #f9f9f9;
    }

    .flexor {
        display: flex;
        align-items: center;
        justify-content: center;
    }
</style>