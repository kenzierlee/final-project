<template>
    <div class="user-boards">
        <nav class="navbar">
            <div class="flexor">
                <router-link :to="{name: 'Home'}">
                    <div>
                        <img class="logo pad-r" src="../assets/nav-logo.png">
                    </div>
                </router-link>
            </div>
            <div v-if="!user.email">
                <div class="dropdown pointer">
                    <a class="dropdown-toggle" role="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <span>
                            <i class="fas fa-sign-in-alt"></i>
                        </span>
                    </a>
                    <div class="dropdown-menu" aria-labelledby="dropdownMenu2">
                        <div class="dropdown-item pointer ddcss" data-toggle="modal" data-target="#signUp">
                            Sign Up
                        </div>
                        <div class="dropdown-item pointer ddcss" data-toggle="modal" data-target="#login">
                            Login
                        </div>
                    </div>
                </div>
            </div>
            <div v-else>
                <div class="dropdown pointer">
                    <a class="dropdown-toggle" role="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <span>{{user.name}}
                            <i class="fas fa-user"></i>
                        </span>
                    </a>
                    <div class="dropdown-menu" aria-labelledby="dropdownMenu2">
                        <router-link :to="{name: 'Home'}">
                            <div class="dropdown-item pointer">
                                <i class="fas fa-home"></i> Home
                            </div>
                        </router-link>
                        <div class="dropdown-item pointer ddcss" data-toggle="modal" data-target="#addPin">
                            <i class="fas fa-thumbtack"></i> Add Pin
                        </div>
                        <div class="dropdown-item pointer ddcss" data-toggle="modal" data-target="#addBoard">
                            <i class="far fa-clipboard"></i> Add Board
                        </div>
                        <!-- <router-link to="/boards"> -->
                        <router-link :to="{name: 'UserPins'}">
                            <div class="dropdown-item pointer">
                                <i class="fas fa-thumbtack"></i> Created Pins
                            </div>
                        </router-link>
                        <!-- </router-link> -->
                        <router-link :to="{name: 'UserBoards'}">
                            <div class="dropdown-item pointer">
                                <i class="far fa-clipboard"></i> Your Boards
                            </div>
                        </router-link>
                        <button @click.prevent="logout" class="dropdown-item pointer ddcss" type="button">
                            <i class="fas fa-sign-out-alt"></i> Logout</button>
                    </div>
                </div>
            </div>
        </nav>
        <!-- SIGN UP MODAL -->
        <div class="modal fade" id="signUp" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content full-width">
                    <img class="mid" src="../assets/main-logo.png" height="150" width="180">
                    <form @submit.prevent="createUser(createdUser)">
                        <div class="modal-body">
                            <div class="m-2">
                                <i class="fas mar-right fa-1x fa-user"></i>
                                <input type="text" name="name" v-model="createdUser.Name" placeholder="Name" required>
                            </div>
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-envelope"></i>
                                <input type="email" name="userEmail" v-model="createdUser.Email" placeholder="Email" required>
                            </div>
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-edit"></i>
                                <input type="password" name="password" v-model="createdUser.Password" placeholder=" Password">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-sm grey-btn" data-dismiss="modal">
                                <b>Close</b>
                            </button>
                            <button type="submit" class="btn btn-sm blue-btn">
                                <b>Create User</b>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- /SIGN UP MODAL -->
        <!-- LOGIN MODAL -->
        <div class="modal fade" id="login" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content full-width">
                    <img class="mid" src="../assets/main-logo.png" height="150" width="180">
                    <form @submit.prevent="login(user)">
                        <div class="modal-body">
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-envelope"></i>
                                <input type="text" name="userEmail" v-model="user.Email" placeholder=" Email" required>
                            </div>
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-edit"></i>
                                <input type="password" name="password" v-model="user.Password" placeholder=" Password">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-sm grey-btn" data-dismiss="modal">
                                <b>Close</b>
                            </button>
                            <button type="submit" class="btn btn-sm blue-btn">
                                <b>Login</b>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- /LOGIN MODAL -->
        <!-- ADD PIN MODAL -->
        <div class="modal fade" id="addPin" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content full-width">
                    <img class="mid" src="../assets/main-logo.png" height="150" width="180">
                    <form @submit.prevent="createPin(pin)">
                        <div class="modal-body">
                            <div class="m-2">
                                <i class="far fa-image mar-right fa-1x"></i>
                                <input type="text" name="pinUrl" v-model="pin.imgUrl" placeholder="Image URL" required>
                            </div>
                            <div class="m-2">
                                <i class="fas fa-external-link-alt mar-right fa-1x"></i>
                                <input type="text" name="pinHref" v-model="pin.href" placeholder="Website URL">
                            </div>
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-edit"></i>
                                <input type="text" name="pinName" v-model="pin.name" placeholder="Pin Name">
                            </div>
                            <div class="m-2">
                                <i class="far mar-right fa-1x fa-edit"></i>
                                <input type="text" name="pinDescription" v-model="pin.description" placeholder="Pin Description">
                            </div>
                            <div>
                                <input type="checkbox" v-model="pin.public" true-value="0" false-value="1">
                                <div v-if="pin.public==0">
                                    <i class="fas fa-lock"></i>
                                </div>
                                <div v-else>
                                    <i class="fas fa-unlock"></i>
                                </div>
                                <p>
                                    Check the box to make pin "Private". If box is left unchecked pin will remain "Public". Once a pin is made "Public", it cannot
                                    be made "Private".
                                </p>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-sm grey-btn" data-dismiss="modal">
                                <b>Close</b>
                            </button>
                            <button type="submit" class="btn btn-sm blue-btn">
                                <b>Add Pin</b>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- /ADD PIN MODAL -->
        <!-- ADD BOARD MODAL -->
        <div class="modal fade" id="addBoard" tabindex="-1" role="dialog">
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
    </div>
</template>

<script>
    import Router from "../router"
    import Home from "./Home"
    export default {
        name: 'Navbar',
        data() {
            return {
                createdUser: {},
                pin: {
                    public: 1
                },
                board: {}
            }
        },
        computed: {
            user() {
                return this.$store.state.user
            }
        },
        methods: {
            createPin() {
                this.pin.public == "0" ? this.pin.public = 0 : this.pin.public = 1;
                this.$store.dispatch('createPin', this.pin);
                $('#addPin').modal('hide');
                this.notify();
                this.pin = {};
            },
            createBoard() {
                this.$store.dispatch('createBoard', this.board);
                $('#addBoard').modal('hide');
                this.notifyBoard();
                this.board = {};
            },
            logout() {
                this.$store.dispatch('logout');
            },
            login() {
                this.$store.dispatch('login', this.user);
                $('#login').modal('hide');
            },
            createUser(createdUser) {
                this.$store.dispatch('createUser', this.createdUser);
                $('#signUp').modal('hide');
                this.createdUser = {}
            },
            notify() {
                alertify.set('notifier', 'position', 'bottom-center');
                alertify.notify('Pin Added!', 'success', 5, function () { console.log('dismissed'); });
            },
            notifyBoard() {
                alertify.set('notifier', 'position', 'bottom-center');
                alertify.notify('Board Added!', 'success', 5, function () { console.log('dismissed'); });
            }
        }
    }

</script>


<style scoped>
    .logo {
        height: 2rem;
    }

    #must-login {
        display: none;
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

    .navbar {
        background-color: #95BAC3;
        /* background-color: #7BA6D0; */
        color: white;
        height: 10vh;
    }

    .navbar h3 {
        font-family: 'Paytone One', sans-serif;
        font-weight: 800;
        font-size: 2rem;
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
        padding-right: 1.5rem;
    }
</style>