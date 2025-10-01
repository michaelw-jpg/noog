import {BrowserRouter, Route, Routes } from 'react-router-dom'
//import HomePage from "./pages/Home/Home.tsx"
import VideoCallSetup from './pages/videoCall/VideoCall.tsx'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<VideoCallSetup />} />
        <Route path="/VideoCallSetup" element={<VideoCallSetup />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App