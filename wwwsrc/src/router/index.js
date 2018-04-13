import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/components/Home'
import UserBoards from '@/components/UserBoards'
import BoardPins from '@/components/BoardPins'
import UserPins from '@/components/UserPins'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/userboards',
      name: 'UserBoards',
      component: UserBoards
    },
    {
      path: '/boardpins/:boardId',
      name: 'BoardPins',
      component: BoardPins
    },
    {
      path: '/userpins',
      name: 'UserPins',
      component: UserPins
    }

  ]
})
