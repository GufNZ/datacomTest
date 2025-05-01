import React, { useState, useEffect } from 'react';

import {
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Paper,
	Box,
	IconButton,
	MenuItem,
	Menu,
	Pagination,
	Stack,
	CircularProgress,
	Alert,
} from '@mui/material';
import { Edit } from '@mui/icons-material';

import { api } from '../services/api';
import { JobApplication } from '../types/JobApplication';
import { ApplicationStatus } from '../types/ApplicationStatus';

interface JobApplicationsListProps {
	onEdit: (application: JobApplication) => void;
}

export const JobApplicationsList: React.FC<JobApplicationsListProps> = ({ onEdit }) => {
	const [applications, setApplications] = useState<JobApplication[]>([]);
	const [page, setPage] = useState(1);
	const [totalPages, setTotalPages] = useState(1);
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState<string | null>(null);
	const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
	const [selectedApplication, setSelectedApplication] = useState<JobApplication | null>(null);

	const fetchApplications = async (page: number = 1) => {
		setLoading(true);
		setError(null);
		try {
			const response = await api.getApplications(page);
			setApplications(response.data);
			setTotalPages(response.pages);
		} catch (err) {
			setError('Failed to fetch applications');
			console.error('Error fetching applications:', err);
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		fetchApplications(page);
	}, [page]);

	const handleEditClick = (event: React.MouseEvent<HTMLElement>, application: JobApplication) => {
		setAnchorEl(event.currentTarget);
		setSelectedApplication(application);
	};

	const handleEditClose = () => {
		setAnchorEl(null);
		setSelectedApplication(null);
	};

	const handleStatusChange = async (status: ApplicationStatus) => {
		if (selectedApplication) {
			const updatedApplication = { ...selectedApplication, status };
			await api.updateApplication(updatedApplication);
			onEdit(updatedApplication);
		}
		handleEditClose();
	};

	const handlePageChange = (_: React.ChangeEvent<unknown>, newPage: number) => {
		setPage(newPage);
	};

	return (
		<Box sx={{ width: '100%' }}>
			{error && (
				<Alert severity="error" sx={{ mb: 2 }}>
					{error}
				</Alert>
			)}
			{loading ? (
				<CircularProgress />
			) : (
				<>
					<TableContainer component={Paper}>
						<Table>
							<TableHead>
								<TableRow>
									<TableCell>Company Name</TableCell>
									<TableCell>Position</TableCell>
									<TableCell>Status</TableCell>
									<TableCell>Date Applied</TableCell>
									<TableCell>Actions</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{applications.map((application) => (
									<TableRow key={application.id}>
										<TableCell>{application.companyName}</TableCell>
										<TableCell>{application.position}</TableCell>
										<TableCell>{application.status}</TableCell>
										<TableCell>{application.dateApplied}</TableCell>
										<TableCell>
											<IconButton
												size="small"
												onClick={(event) => handleEditClick(event, application)}
											>
												<Edit />
											</IconButton>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					</TableContainer>
					{totalPages > 1 && (
						<Stack
							sx={{ mt: 2 }}
							direction="row"
							justifyContent="center"
						>
							<Pagination
								count={totalPages}
								page={page}
								onChange={handlePageChange}
								color="primary"
							/>
						</Stack>
					)}
					<Menu
						anchorEl={anchorEl}
						open={Boolean(anchorEl)}
						onClose={handleEditClose}
					>
						<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Applied)}>Applied</MenuItem>
						<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Interview)}>Interview</MenuItem>
						<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Offer)}>Offer</MenuItem>
						<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Rejected)}>Rejected</MenuItem>
					</Menu>
				</>
			)}
		</Box>
	);
};
