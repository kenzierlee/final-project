import vue from "vue";
import vuex from "vuex";
import axios from "axios";
import router from "../router/index";

var auth = axios.create({
    baseURL: "//localhost:5000/account/",
    withCredentials: true
});

var api = axios.create({
    baseURL: "//localhost:5000/api/",
    withCredentials: true
});

vue.use(vuex);

export default new vuex.Store({
    state: {
        user: {},
        pins: [],
        boards: [],
        boardPins: [],
        userPins: []
    },
    mutations: {
        updateUser(state, payload) {
            state.user = payload;
        },
        setPins(state, payload) {
            state.pins = payload;
        },
        setUserPins(state, payload) {
            state.userPins = payload;
        },
        setBoards(state, payload) {
            state.boards = payload;
        },
        setBoardPins(state, payload) {
            state.boardPins = payload;
        }
    },
    actions: {
        getBoards({ commit, dispatch }) {
            api.get("boards").then(res => {
                commit("setBoards", res.data)
            }).catch(err => {
                console.log(err)
            })
        },
        createBoard({ commit, dispatch }, payload) {
            api.post("boards", payload).then(res => {
                dispatch("getBoards")
            }).catch(err => {
                console.log(err)
            })
        },
        editBoard({ commit, dispatch }, payload) {
            api.put("boards/" + payload.id, payload).then(res => {
                dispatch("getBoards")
            }).catch(err => {
                console.log(err)
            })
        },
        getPins({ commit, dispatch }) {
            api.get("pins").then(res => {
                commit("setPins", res.data)
            }).catch(err => {
                console.log(err)
            })
        },
        getUserPins({ commit, dispatch }) {
            api.get("pins/userpins").then(res => {
                commit("setUserPins", res.data)
            }).catch(err => {
                console.log(err)
            })
        },
        getBoardPins({ commit, dispatch }, payload) {
            api.get("pins/" + payload).then(res => {
                commit("setBoardPins", res.data)
            }).catch(err => {
                console.log(err)
            })
        },
        createPin({ commit, dispatch }, payload) {
            api.post("pins", payload).then(res => {
                dispatch("getPins")
            }).catch(err => {
                console.log(err)
            })
        },
        savePin({ commit, dispatch }, payload) {
            api.post("pins/savepin", payload).then(res => {
                dispatch("getBoardPins", payload.boardId)
            }).catch(err => {
                console.log(err)
            })
        },
        editPin({ commit, dispatch }, payload) {
            api.put(payload.id, payload).then(res => {
                dispatch("getPins")
            }).catch(err => {
                console.log(err)
            })
        },
        deletePin({ commit, dispatch }, payload) {
            api.delete("pins/"+payload).then(res => {
                dispatch("getPins")
                dispatch("getUserPins")
            }).catch(err => {
                console.log(err)
            })
        },
        deleteBoardPin({ commit, dispatch }, payload) {
            api.delete("pins/boardpin/"+payload.id).then(res => {
                dispatch("getBoardPins", payload.boardId)
            }).catch(err => {
                console.log(err)
            })
        },
        updateViews({ commit, dispatch }, payload) {
            api.put("pins/views/" + payload).then(res => {
                dispatch("getPins")
            }).catch(err => {
                console.log(err)
            })
        },
        createUser({ commit, dispatch }, payload) {
            auth.post("register", payload).then(res => {
                commit("updateUser", res.data)
            })
                .catch(err => {
                    console.log(err)
                })
        },
        login({ commit, dispatch }, payload) {
            auth.post("login", payload).then(res => {
                commit("updateUser", res.data.user)
            })
                .catch(err => {
                    console.log("Invalid Credentials")
                })
        },
        authenticate({ commit, dispatch }, payload) {
            auth.get("authenticate").then(res => {
                commit("updateUser", res.data)
            })
                .catch(err => {
                    console.log("Invalid Credentials")
                })
        },
        logout({ commit, dispatch }, payload) {
            auth.delete("logout").then(res => {
                commit("updateUser", {})
            })
                .catch(err => {
                    console.log(err)
                })
        }
    }
})