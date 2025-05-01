import React, { useEffect, useState, useCallback } from 'react';
import debounce from 'lodash/debounce';

import {
	Alert,
	Button,
	CircularProgress,
	Dialog,
	DialogActions,
	DialogContent,
	DialogContentText,
	DialogTitle,
	IconButton,
	InputLabel,
	Menu,
	MenuItem,
	Paper,
	Pagination,
	Select,
	Stack,
	styled,
	Table,
	TableBody,
	TableContainer,
	TableHead,
	TableRow,
	TableCell,
	TableSortLabel,
	TextField,
	FormControl,
} from '@mui/material';
import { Edit, Delete } from '@mui/icons-material';

import { api } from '../services/api';
import { ApplicationStatus } from '../types/ApplicationStatus';
import { JobApplication } from '../types/JobApplication';
import { SortKey } from '../types/SortKey';

interface JobApplicationsListProps {
	onEdit: (application: JobApplication) => void;
}

interface FilterState {
	status: ApplicationStatus | undefined;
	company: string;
	position: string;
}

interface TableColumn {
	id: string;
	label: string;
	align: 'left' | 'right' | 'center';
	render: (application: JobApplication) => React.ReactNode;
	sortable?: boolean;
}

const StyledTableCell = styled(TableCell)({
	fontWeight: 'bold',
});

export const JobApplicationsList: React.FC<JobApplicationsListProps> = ({ onEdit }) => {
	const [page, setPage] = useState(1);
	const [totalPages, setTotalPages] = useState(1);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
	const [selectedApplication, setSelectedApplication] = useState<JobApplication | null>(null);
	const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
	const [deleteApplicationId, setDeleteApplicationId] = useState<number | null>(null);
	const [applications, setApplications] = useState<JobApplication[]>([]);
	const [filters, setFilters] = useState<FilterState>({
		status: undefined,
		company: '',
		position: '',
	});
	const [sortConfig, setSortConfig] = useState<{ key: SortKey | null; direction: 'asc' | 'desc' }>({ key: null, direction: 'asc' });

	const fetchApplications = useCallback(
		async (page: number = 1, filters: FilterState) => {
			const { key, direction } = sortConfig;
			setLoading(true);
			setError(null);
			try {
				const response = await api.getApplications({
					...filters,
					sortBy: key ?? undefined,
					sortDirection: direction,
					page,
				});
				setApplications(response.data);
				setTotalPages(response.pages);
				// Reset to first page if current page is out of bounds:
				if (page > response.pages) {
					setPage(1);
				}
			} catch (err) {
				setError('Failed to fetch applications!');
				console.error('Error fetching applications:', err);
			} finally {
				setLoading(false);
			}
		},
		[sortConfig]
	);

	useEffect(
		() => {
			fetchApplications(page, filters);
		},
		[page, filters, fetchApplications]
	);

	const debouncedFetch = useCallback(
		(page: number, filters: FilterState) => {
			return debounce(() => {
				fetchApplications(page, filters);
			}, 500);
		},
		[fetchApplications]
	) as (page: number, filters: FilterState) => void;

	const handleFilterChange = (field: keyof FilterState) =>
		(value: string | ApplicationStatus | null) => {
			setFilters(
				(prev) => ({
					...prev,
					[field]: value,
				})
			);

			debouncedFetch(page, filters);
		};

	const handleSort = (key: SortKey) => {
		if (key === sortConfig.key) {
			setSortConfig({
				key,
				direction: sortConfig.direction === 'asc' ? 'desc' : 'asc',
			});
		} else {
			setSortConfig({
				key,
				direction: 'asc',
			});
		}

		debouncedFetch(page, filters);
	};

	const handleEditClick = (event: React.MouseEvent<HTMLButtonElement>, application: JobApplication) => {
		setAnchorEl(event.currentTarget);
		setSelectedApplication(application);
	};

	const handleDeleteClick = (application: JobApplication) => {
		setDeleteApplicationId(application.id);
		setDeleteDialogOpen(true);
	};

	const handleDeleteConfirm = async () => {
		if (deleteApplicationId) {
			try {
				await api.deleteApplication(deleteApplicationId);
				setApplications(applications.filter(app => app.id !== deleteApplicationId));
				setDeleteDialogOpen(false);
				setDeleteApplicationId(null);
			} catch (error) {
				setError('Failed to delete application!');
				console.error('Error deleting application:', error);
			}
		}
	};

	const handleDeleteCancel = () => {
		setDeleteDialogOpen(false);
		setDeleteApplicationId(null);
	};

	const handleEditClose = () => {
		setAnchorEl(null);
		setSelectedApplication(null);
	};

	const handleStatusChange = async (status: ApplicationStatus) => {
		if (selectedApplication) {
			const updatedApplication = { ...selectedApplication, status };
			await api.updateApplication(updatedApplication);
			setApplications(applications.map(app =>
				app.id === updatedApplication.id ? updatedApplication : app
			));
			setSelectedApplication(updatedApplication);
		}
		handleEditClose();
	};

	const headers: TableColumn[] = [
		{ id: SortKey.CompanyName, label: 'Company Name', align: 'left', render: (app) => app.companyName, sortable: true },
		{ id: SortKey.Position, label: 'Position', align: 'left', render: (app) => app.position, sortable: true },
		{ id: SortKey.Status, label: 'Status', align: 'left', render: (app) => app.status, sortable: true },
		{ id: SortKey.DateApplied, label: 'Date Applied', align: 'left', render: (app) => app.dateApplied, sortable: true },
		{
			id: 'actions',
			label: 'Actions',
			align: 'center',
			render: (app) => (
				<Stack direction="row" spacing={1}>
					<IconButton size="small" onClick={(event) => handleEditClick(event, app)}>
						<Edit/>
					</IconButton>
					<IconButton size="small" onClick={() => handleDeleteClick(app)} color="error">
						<Delete/>
					</IconButton>
				</Stack>
			)
		},
	];

	return (
		<Stack spacing={2}>
			<Stack direction="row" spacing={2}>
				<FormControl fullWidth>
					<InputLabel>Status</InputLabel>
					<Select
						value={filters.status || ''}
						onChange={(event) => handleFilterChange('status')(event.target.value)}
						label="Status"
					>
						<MenuItem value="">All</MenuItem>
						<MenuItem value={ApplicationStatus.Applied}>Applied</MenuItem>
						<MenuItem value={ApplicationStatus.Interview}>Interview</MenuItem>
						<MenuItem value={ApplicationStatus.Offer}>Offer</MenuItem>
						<MenuItem value={ApplicationStatus.Rejected}>Rejected</MenuItem>
					</Select>
				</FormControl>
				<TextField
					fullWidth
					label="Company"
					value={filters.company}
					onChange={(e) => handleFilterChange('company')(e.target.value)}
					variant="outlined"
				/>
				<TextField
					fullWidth
					label="Position"
					value={filters.position}
					onChange={(e) => handleFilterChange('position')(e.target.value)}
					variant="outlined"
				/>
			</Stack>
			{error && <Alert severity="error">{error}</Alert>}
			{loading ? (
				<CircularProgress sx={{ mt: 2 }} />
			) : error ? null : (
				<>
					<TableContainer component={Paper}>
						<Table>
							<TableHead>
								<TableRow>
										{headers.map(
											(header) => header.sortable
												? (
													<StyledTableCell key={header.id} align={header.align}>
														<TableSortLabel
															active={sortConfig.key === header.id}
															direction={sortConfig.key === header.id ? sortConfig.direction : 'asc'}
															onClick={() => handleSort(header.id as SortKey)}
														>
															{header.label}
														</TableSortLabel>
													</StyledTableCell>
												) : (
													<StyledTableCell key={header.id} align={header.align}>
														{header.label}
													</StyledTableCell>
												)
											)}
								</TableRow>
							</TableHead>
							<TableBody>
								{applications.map((application) => (
									<TableRow key={application.id}>
										{headers.map((header) => (
											<TableCell key={header.id} align={header.align}>
												{header.render(application)}
											</TableCell>
										))}
									</TableRow>
								))}
							</TableBody>
						</Table>
					</TableContainer>
					{totalPages > 1 && (
						<Pagination
							count={totalPages}
							page={page}
							onChange={(_, newPage) => setPage(newPage)}
							color="primary"
						/>
					)}
				</>
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
			<Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
				<DialogTitle>Delete Application</DialogTitle>
				<DialogContent>
					<DialogContentText>
						Are you sure you want to delete this application?
					</DialogContentText>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleDeleteCancel} color="primary">
						Cancel
					</Button>
					<Button onClick={handleDeleteConfirm} color="error">
						Delete
					</Button>
				</DialogActions>
			</Dialog>
		</Stack>
	);
};
