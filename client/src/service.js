import axios from 'axios';

// Config default base URL API requests
const apiUrl = process.env.REACT_APP_API_ADDRESS;
axios.defaults.baseURL = apiUrl;
// axios.defaults.headers.common = {
//   'Content-Type': 'application/json',
//   'Access-Control-Allow-Origin': '*',
//   //'Access-Control-Allow-Credentials': 'true',
//   'Access-Control-Allow-Methods': 'OPTIONS, GET, POST',
//   'Access-Control-Allow-Headers': 'Content-Type, Depth, User'
// };
// axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';

const writeToLog = (error) => {
  console.error('Error :', error);
};

axios.interceptors.response.use(
  (response) => response,
  (error) => {   
    writeToLog(error);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    try{
      const result = await axios.get(`${apiUrl}/items`,{ crossDomain: true });    
      return result.data;
    }
    catch (error) {
      console.error('Error in getTasks:', error);
      throw error; }

  },

  addTask: async(name)=>{
    console.log('addTask', name)
    try{
    const result = await axios.post(`${apiUrl}/items?name=${name}`, { crossDomain: true });  
    return result.data;
    }
    catch (error) {  
      console.error('Error in addTask:', error);
      throw error; 
    }
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    try{
      var result = await axios.put(`${apiUrl}/items/${id}?complete=${isComplete}`,{ crossDomain: true });
      return result;
    }
    catch (error) {
      console.error('Error in setCompleted:', error);
      throw error; 
    }
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    try{
      var result = await axios.delete(`${apiUrl}/items/${id}`,{ crossDomain: true });
      return result;
    }
    catch(error){
      console.error('Error in deleteTask:', error);
      throw error;
    }
  }
};
