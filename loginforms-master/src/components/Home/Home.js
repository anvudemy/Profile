import React,{ useState,useEffect } from 'react';
import { withRouter } from 'react-router-dom';
import { ACCESS_TOKEN_NAME, API_BASE_URL } from '../../constants/apiConstants';
import axios from 'axios'

var temp;
function Home(props) {
  const [user, setUser] = useState();
    useEffect(() => {
     // this.setState({...this.state, isFetching: true});
        axios.get('http://localhost:5000/api/user/UserDetails', { headers: { 'Authorization': `Bearer `+localStorage.getItem(ACCESS_TOKEN_NAME) }})
        .then((response)=> setUser(response.data.fullName))           
        .catch(function (error) {
          redirectToLogin()
        })
        console.log(user);
      })
      
    function redirectToLogin() {
    props.history.push('/login');
    }
    return(
        <div className="mt-2">
        <h1>Welcome!!!</h1>
         <br/>
         <h2>{user}</h2>
        </div>
    )

    }
export default withRouter(Home);