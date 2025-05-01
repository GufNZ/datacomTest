import axios from 'axios';

import { JobApplication } from '../types/JobApplication';

const API_URL = 'http://localhost:5000/api/applications';

export const api = {
	getApplications: async () => {
		const response = await axios.get(API_URL);
		return response.data;
	},
	getApplication: async (id: number) => {
		const response = await axios.get(`${API_URL}/${id}`);
		return response.data;
	},
	createApplication: async (application: Omit<JobApplication, 'id'>) => {
		const response = await axios.post(API_URL, application);
		return response.data;
	},
	updateApplication: async (application: JobApplication) => {
		const response = await axios.put(`${API_URL}/${application.id}`, application);
		return response.data;
	}
};
