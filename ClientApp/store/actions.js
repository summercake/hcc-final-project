import axios from 'axios'
import oktaAuth from '../oktaAuth'
axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';


const sleep  = ms => {
  return new Promise(resolve => setTimeout(resolve, ms))
}

const addAuthHeader = () => {
  return {
    headers: {
        'Authorization': 'Bearer '
            + oktaAuth.client.tokenManager.get('access_token').accessToken
    }
  }
}

export const actions = {
  checkLoggedIn({ commit }) {
    if (oktaAuth.client.tokenManager.get('access_token')) {
      let idToken = oktaAuth.client.tokenManager.get('id_token')
      commit('loggedIn', idToken.claims)
    }
  },

  async login({ dispatch, commit }, data) {
    let authResponse
    try {
      authResponse = await oktaAuth.client.signIn({
        username: data.email,
        password: data.password
      });
    }
    catch (err) {
      let message = err.message || 'Login error'
      dispatch('loginFailed', message)
      return
    }

    if (authResponse.status !== 'SUCCESS') {
      console.error("Login unsuccessful, or more info required", response.status)
      dispatch('loginFailed', 'Login error')
      return
    }

    let tokens
    try {
      tokens = await oktaAuth.client.token.getWithoutPrompt({
        responseType: ['id_token', 'token'],
        scopes: ['openid', 'email', 'profile'],
        sessionToken: authResponse.sessionToken,
      })
    }
    catch (err) {
      let message = err.message || 'Login error'
      dispatch('loginFailed', message)
      return
    }

    // Verify ID token validity
    try {
      await oktaAuth.client.token.verify(tokens[0])
    } catch (err) {
      dispatch('loginFailed', 'An error occurred')
      console.error('id_token failed validation')
      return
    }

    oktaAuth.client.tokenManager.add('id_token', tokens[0]);
    oktaAuth.client.tokenManager.add('access_token', tokens[1]);

    commit('loggedIn', tokens[0].claims)
  },

  async logout({ commit }) {
    oktaAuth.client.tokenManager.clear()
    await oktaAuth.client.signOut()
    commit('loggedOut')
  },

  async loginFailed({ commit }, message) {
    commit('loginError', message)
    await sleep(3000)
    commit('loginError', null)
  },

  async getAllBlogs({ commit }) {
    let response = await axios.get('/api/blog', addAuthHeader())
    
    if (response && response.data) {
      let updatedBlogs = response.data
      commit('loadBlogs', updatedBlogs)
    }
  },

  async addBlog({ dispatch }, data) {
    // var params = new URLSearchParams();
    // params.append('Title', data.title);
    // params.append('Content', 'value2');
    await axios.post(
      '/api/blog',
      { Title: data.title, Content:data.content },
      addAuthHeader()).then(function(){
        console.log(123)
      }).catch(function(){
        console.log(data)
        console.log(456)
      })
    await dispatch('getAllBlogs')
  },

  async toggleBlog({ dispatch }, data) {
    await axios.post(
      '/api/blog/' + data.id,
      { completed: data.completed },
      addAuthHeader())

    await dispatch('getAllBlogs')
  },

  async deleteBlog({ dispatch }, id) {
    await axios.delete('/api/blog/' + id, addAuthHeader())
    await dispatch('getAllBlogs')
  }
}
