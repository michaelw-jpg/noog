import React from 'react'
import { useNavigate } from 'react-router-dom'
import './Home.css'

const HomePage: React.FC = () => {
  const navigate = useNavigate();



  return (
    <div className= "home-container">
      <h1>Välj en sida</h1>
      <div className='button-container'>
        <button onClick={() => navigate("/")}>Gå till Hem</button>
        <button onClick={() => navigate("/VideoCallSetup")}>Gå till video</button>
      </div>
    </div>

  )
}
export default HomePage