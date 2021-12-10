import axios from "axios";
import { BASE_URL_API } from "../../constants/constants";

class Axios {
  private baseApiUrl: string | null;

  constructor() {
    this.baseApiUrl = BASE_URL_API + "api";
  }

  public async getData(
    apiSuffix: string,
    configurations?: { [key: string]: any }
  ) {
    const apiUrl: string = this.baseApiUrl + apiSuffix;
    const { data, status } = await axios.get(apiUrl, {
      ...configurations,
    });
    return { data, status };
  }

  public async postData(apiSuffix: string, postData: object) {
    const apiUrl: string = this.baseApiUrl + apiSuffix;
    const { data, status } = await axios.post(apiUrl, postData);
    return { data, status };
  }

  public async putData(apiSuffix: string, putData: object) {
    const apiUrl: string = this.baseApiUrl + apiSuffix;
    const { data, status } = await axios.put(apiUrl, putData);
    return { data, status };
  }

  public async deleteData(apiSuffix: string, deleteData: object) {
    const apiUrl: string = this.baseApiUrl + apiSuffix;
    
    const { data, status } = await axios.delete(apiUrl, { data: deleteData });
    return { data, status };
  }
}

const axiosApi: Axios = new Axios();
export default axiosApi;
