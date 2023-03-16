
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEditor.PackageManager.Requests;
//using UnityEngine;

///**
// * 通过gitlab Api自动下载gitLab上的所有项目
// */

//public class GitlabProjectCloneService
//{

//    const string gitlabUrl = "139.217.219.98/cvdgroup/equips";


//    const string privateToken = "sXYE8Zq27HSPsLPuKVoU";


//    const string projectDir;

//    ObjectMapper objectMapper = new ObjectMapper();

//    @Autowired
//    RestTemplate restTemplate;

//    @PostConstruct
//    private void start()
//    {
//        File execDir = new File(projectDir);
//        System.out.println("start get gitlab projects");
//        List<GitGroup> groups = getGroups();
//        try
//        {
//            System.out.println(objectMapper.writeValueAsString(groups));
//        }
//        catch (JsonProcessingException e)
//        {
//            e.printStackTrace();
//        }
//        for (GitGroup group : groups)
//        {
//            List<GitProject> projects = getProjectsByGroup(group.getName());
//            for (GitProject project : projects)
//            {
//                String lastActivityBranchName = getLastActivityBranchName(project.getId());
//                if (StringUtils.isEmpty(lastActivityBranchName))
//                {
//                    System.out.println("branches is empty, break project...");
//                    continue;
//                }
//                clone(lastActivityBranchName, project, execDir);
//            }
//        }
//        System.out.println("end get gitlab projects");
//    }

//    /**
//     * 获取所有项目
//     *
//     * @return
//     */
//    private List<GitProject> getAllProjects()
//    {
//        String url = gitlabUrl + "/api/v3/projects?per_page={per_page}&private_token={private_token}";
//        Map<String, String> uriVariables = new HashMap<>();
//        uriVariables.put("per_page", "100");
//        uriVariables.put("private_token", privateToken);
//        HttpHeaders headers = new HttpHeaders();
//        headers.setContentType(MediaType.APPLICATION_JSON);

//        HttpEntity entity = new HttpEntity<>(headers);
//        ParameterizedTypeReference<List<GitProject>> responseType = new ParameterizedTypeReference<List<GitProject>>()
//        {
//        };
//        ResponseEntity<List<GitProject>> responseEntity = restTemplate.exchange(url, HttpMethod.GET, entity, responseType, uriVariables);
//        if (HttpStatus.OK == responseEntity.getStatusCode())
//        {
//            return responseEntity.getBody();
//        }
//        return null;
//    }

//    /**
//     * 获取指定分组下的项目
//     *
//     * @param group
//     * @return
//     */
//    private List<GitProject> getProjectsByGroup(String group)
//    {
//        String url = gitlabUrl + "/api/v3/groups/{group}/projects?per_page={per_page}&private_token={private_token}";
//        Map<String, String> uriVariables = new HashMap<>();
//        uriVariables.put("group", group);
//        uriVariables.put("per_page", "100");
//        uriVariables.put("private_token", privateToken);
//        HttpHeaders headers = new HttpHeaders();
//        headers.setContentType(MediaType.APPLICATION_JSON);

//        HttpEntity entity = new HttpEntity<>(headers);
//        ParameterizedTypeReference<List<GitProject>> responseType = new ParameterizedTypeReference<List<GitProject>>()
//        {
//        };
//        ResponseEntity<List<GitProject>> responseEntity = restTemplate.exchange(url, HttpMethod.GET, entity, responseType, uriVariables);
//        if (HttpStatus.OK == responseEntity.getStatusCode())
//        {
//            return responseEntity.getBody();
//        }
//        return null;
//    }

//    /**
//     * 获取分组列表
//     *
//     * @return
//     */
//    private List<GitGroup> getGroups()
//    {
//        String url = gitlabUrl + "/api/v3/groups?private_token={private_token}";
//        Map<String, String> uriVariables = new HashMap<>();
//        uriVariables.put("private_token", privateToken);
//        HttpHeaders headers = new HttpHeaders();
//        headers.setContentType(MediaType.APPLICATION_JSON);

//        HttpEntity entity = new HttpEntity<>(headers);
//        ParameterizedTypeReference<List<GitGroup>> responseType = new ParameterizedTypeReference<List<GitGroup>>()
//        {
//        };
//        ResponseEntity<List<GitGroup>> responseEntity = restTemplate.exchange(url, HttpMethod.GET, entity, responseType, uriVariables);
//        if (HttpStatus.OK == responseEntity.getStatusCode())
//        {
//            return responseEntity.getBody();
//        }
//        return null;
//    }

//    /**
//     * 获取最近修改的分支名称
//     *
//     * @param projectId 项目ID
//     * @return
//     */
//    private String getLastActivityBranchName(Long projectId)
//    {
//        List<GitBranch> branches = getBranches(projectId);
//        if (CollectionUtils.isEmpty(branches))
//        {
//            return "";
//        }
//        GitBranch gitBranch = getLastActivityBranch(branches);
//        return gitBranch.getName();
//    }

//    /**
//     * 获取指定项目的分支列表
//     * https://docs.gitlab.com/ee/api/branches.html#branches-api
//     *
//     * @param projectId 项目ID
//     * @return
//     */
//    private List<GitBranch> getBranches(Long projectId)
//    {
//        String url = gitlabUrl + "/api/v3/projects/{projectId}/repository/branches?private_token={privateToken}";
//        Map<String, Object> uriVariables = new HashMap<>();
//        uriVariables.put("projectId", projectId);
//        uriVariables.put("privateToken", privateToken);
//        HttpHeaders headers = new HttpHeaders();
//        headers.setContentType(MediaType.APPLICATION_JSON);

//        HttpEntity entity = new HttpEntity<>(headers);
//        ParameterizedTypeReference<List<GitBranch>> responseType = new ParameterizedTypeReference<List<GitBranch>>()
//        {
//        };
//        ResponseEntity<List<GitBranch>> responseEntity = restTemplate.exchange(url, HttpMethod.GET, entity, responseType, uriVariables);
//        if (HttpStatus.OK == responseEntity.getStatusCode())
//        {
//            return responseEntity.getBody();
//        }
//        return null;
//    }

//    /**
//     * 获取最近修改的分支
//     *
//     * @param gitBranches 分支列表
//     * @return
//     */
//    private GitBranch getLastActivityBranch(final List<GitBranch> gitBranches)
//    {
//        GitBranch lastActivityBranch = gitBranches.get(0);
//        for (GitBranch gitBranch : gitBranches)
//        {
//            if (gitBranch.getCommit().getCommittedDate().getTime() > lastActivityBranch.getCommit().getCommittedDate().getTime())
//            {
//                lastActivityBranch = gitBranch;
//            }
//        }
//        return lastActivityBranch;
//    }

//    private void clone(String branchName, GitProject gitProject, File execDir)
//    {
//        String command = String.format("git clone -b %s %s %s", branchName, gitProject.getHttpUrlToRepo(), gitProject.getPathWithNamespace());
//        System.out.println("start exec command : " + command);
//        try
//        {
//            Process exec = Runtime.getRuntime().exec(command, null, execDir);
//            exec.waitFor();
//            String successResult = inputStreamToString(exec.getInputStream());
//            String errorResult = inputStreamToString(exec.getErrorStream());
//            System.out.println("successResult: " + successResult);
//            System.out.println("errorResult: " + errorResult);
//            System.out.println("================================");
//        }
//        catch (Exception e)
//        {
//            e.printStackTrace();
//        }
//    }

//    private String inputStreamToString(final InputStream input)
//    {
//        StringBuilder result = new StringBuilder();
//        Reader reader = new InputStreamReader(input);
//        BufferedReader bf = new BufferedReader(reader);
//        String line;
//        try
//        {
//            while ((line = bf.readLine()) != null)
//            {
//                result.append(line);
//            }
//        }
//        catch (IOException e)
//        {
//            e.printStackTrace();
//        }
//        return result.toString();
//    }
//}

