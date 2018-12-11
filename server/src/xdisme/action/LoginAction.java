package xdisme.action;

import com.opensymphony.xwork2.ActionChainResult;
import com.opensymphony.xwork2.ActionSupport;
import org.apache.struts2.ServletActionContext;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.AnnotationConfiguration;
import org.hibernate.cfg.Configuration;
import xdisme.model.JsonData;
import xdisme.model.User;
import xdisme.utils.ResponseJsonUtils;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static com.opensymphony.xwork2.Action.INPUT;
import static com.opensymphony.xwork2.Action.SUCCESS;

public class LoginAction extends ActionChainResult {

    private String account;
    private String password;

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getAccount() {
        return account;
    }

    public void setAccount(String account) {
        this.account = account;
    }


    private static final SessionFactory ourSessionFactory;
    private static Session session;
    static {
        try {
            Configuration configuration = new AnnotationConfiguration();
            configuration.configure();

            ourSessionFactory = configuration.buildSessionFactory();
        } catch (Throwable ex) {
            throw new ExceptionInInitializerError(ex);
        }
    }
    public List<User> get(){
        //数据库中进行查询

        Transaction t = session.beginTransaction();
        List<User> users = (List<User>)session.createQuery("select id from User user where account = ? and nickname = ?")
                .setParameter(0, account)
                .setParameter(1, password)
                .list();
        t.commit();
        return users;
    }
    public void update(int id){
        //数据库中进行查询
        Transaction t = session.beginTransaction();
        User user = (User)session.get(User.class, id);
        user.setOnline("1");
        session.save(user);
        t.commit();
    }

    //解析请求的Json数据
    private String getRequestPostData(HttpServletRequest request) throws IOException {
        int contentLength = request.getContentLength();
        if(contentLength<0){
            return null;
        }
        byte buffer[] = new byte[contentLength];
        for (int i = 0; i < contentLength;) {
            int len = request.getInputStream().read(buffer, i, contentLength - i);
            if (len == -1) {
                break;
            }
            i += len;
        }
        return new String(buffer, "utf-8");
    }




    public void login(){
       session =  ourSessionFactory.openSession();
        HttpServletResponse response = ServletActionContext.getResponse();
       HttpServletRequest request =  ServletActionContext.getRequest();
        String jsonStr  = null;
        try {
            jsonStr = getRequestPostData(request);
        } catch (IOException e) {
            e.printStackTrace();
        }
        System.out.println("type:"+request.getMethod());
        System.out.println("jsonStr:"+jsonStr);
        Map<String, Object> data = new HashMap<String, Object>();
        System.out.println("account"+account);
        System.out.println("password"+password);
        //创建json数据对象
        JsonData jsonData = new JsonData();
        jsonData.setType("1");
        jsonData.setData("1");
        List<User> users = get();

        //用户名存在
        if(users.size() == 1){
            // todo 设置在线为 1
            System.out.println("hello");
            int id = Integer.parseInt(users.get(0)+"");
            System.out.println("id"+id);
           update(id);
            jsonData.setCode("1");
        } //用户名不存在
        else{
            jsonData.setCode("1002");
        }
        session.close();

        ResponseJsonUtils.json(response, jsonData);
        System.out.println(data.toString());
    }
}
