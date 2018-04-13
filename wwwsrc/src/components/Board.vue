<template>
    <div class="board">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">{{board.name}}</h5>
                    <p class="card-text">{{board.description}}</p>
                    <div class="expand">
                        <i class="far fa-edit" data-toggle="modal" :data-target="'#'+board.id"></i>
                        <router-link :to="{name: 'BoardPins', params: {boardId: board.id}}" class="link">
                            <i class="fas fa-expand-arrows-alt"></i>
                        </router-link>
                    </div>
                </div>
            </div>
        <!-- EDIT BOARD MODAL -->
        <div class="modal fade" :id="board.id" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content full-width">
                    <img class="mid" src="../assets/main-logo.png" height="150" width="180">
                    <form @submit.prevent="editBoard(board)">
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
                                <b>Edit Board</b>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- /EDIT BOARD MODAL -->
    </div>
</template>
<script>
    export default {
        name: 'Boards',
        props: ['board'],
        data() {
            return {
            }
        },
        mounted() {
            this.$store.dispatch("getBoards");
            this.$store.dispatch("authenticate");
        },
        methods: {
            editBoard(board){
                this.$store.dispatch("editBoard", board);
                $('#'+board.id).modal('hide');
            }
        },
        computed: {
            boards() {
                return this.$store.state.boards;
            }
        }
    }
</script>
<style scoped>
        .card {
        position: relative;
        display: inline-block;
    }

    .card:hover .expand {
        display: block;
        cursor: pointer;
    }

    .expand {
        padding-top: 7px;
        padding-right: 7px;
        position: absolute;
        right: 0;
        top: 0;
        display: none;
    }

    .expand a {
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
        align-items: flex-end;
        justify-content: space-around;
    }

    .flexor h3 {
        margin-bottom: 0;
    }

    .pad-r {
        padding-right: 1rem;
    }
</style>